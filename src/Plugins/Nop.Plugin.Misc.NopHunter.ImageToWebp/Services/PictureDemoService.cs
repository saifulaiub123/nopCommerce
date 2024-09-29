using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Seo;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace Nop.Plugin.Misc.NopHunter.ImageToWebp.Services;
public class PictureDemoService : PictureService
{
    #region Fields

    protected readonly IDownloadService _downloadService;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly ILogger _logger;
    protected readonly INopFileProvider _fileProvider;
    protected readonly IProductAttributeParser _productAttributeParser;
    protected readonly IProductAttributeService _productAttributeService;
    protected readonly IRepository<Picture> _pictureRepository;
    protected readonly IRepository<PictureBinary> _pictureBinaryRepository;
    protected readonly IRepository<ProductPicture> _productPictureRepository;
    protected readonly ISettingService _settingService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IWebHelper _webHelper;
    protected readonly MediaSettings _mediaSettings;

    #endregion

    #region Ctor

    public PictureDemoService(IDownloadService downloadService,
        IHttpContextAccessor httpContextAccessor,
        ILogger logger,
        INopFileProvider fileProvider,
        IProductAttributeParser productAttributeParser,
        IProductAttributeService productAttributeService,
        IRepository<Picture> pictureRepository,
        IRepository<PictureBinary> pictureBinaryRepository,
        IRepository<ProductPicture> productPictureRepository,
        ISettingService settingService,
        IUrlRecordService urlRecordService,
        IWebHelper webHelper,
        MediaSettings mediaSettings) 
        : base(
            downloadService,
            httpContextAccessor,
            logger,
            fileProvider,
            productAttributeParser,
            productAttributeService,
            pictureRepository,
            pictureBinaryRepository,
            productPictureRepository,
            settingService,
            urlRecordService,
            webHelper,
            mediaSettings
            )
    {
        _downloadService = downloadService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _fileProvider = fileProvider;
        _productAttributeParser = productAttributeParser;
        _productAttributeService = productAttributeService;
        _pictureRepository = pictureRepository;
        _pictureBinaryRepository = pictureBinaryRepository;
        _productPictureRepository = productPictureRepository;
        _settingService = settingService;
        _urlRecordService = urlRecordService;
        _webHelper = webHelper;
        _mediaSettings = mediaSettings;
    }

    #endregion

    #region Method

    protected override async Task<PictureBinary> UpdatePictureBinaryAsync(Picture picture, byte[] binaryData)
    {
        ArgumentNullException.ThrowIfNull(picture);

        var pictureBinary = await GetPictureBinaryByPictureIdAsync(picture.Id);

        var isNew = pictureBinary == null;

        if (isNew)
            pictureBinary = new PictureBinary
            {
                PictureId = picture.Id
            };

        using var inStream = new MemoryStream(binaryData);

        using var myImage = await Image.LoadAsync(inStream);

        using var outStream = new MemoryStream();

        await myImage.SaveAsync(outStream, new WebpEncoder());

        pictureBinary.BinaryData = outStream.ToArray();

        if (isNew)
            await _pictureBinaryRepository.InsertAsync(pictureBinary);
        else
            await _pictureBinaryRepository.UpdateAsync(pictureBinary);

        return pictureBinary;
    }

    public override async Task<Picture> InsertPictureAsync(IFormFile formFile, string defaultFileName = "", string virtualPath = "")
    {
        var imgExt = new List<string>
        {
            ".bmp",
            ".gif",
            ".webp",
            ".jpeg",
            ".jpg",
            ".jpe",
            ".jfif",
            ".pjpeg",
            ".pjp",
            ".png",
            ".tiff",
            ".tif",
            ".svg"
        } as IReadOnlyCollection<string>;

        var fileName = formFile.FileName;
        if (string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(defaultFileName))
            fileName = defaultFileName;

        //remove path (passed in IE)
        fileName = _fileProvider.GetFileName(fileName);

        var contentType = "image/webp";

        var fileExtension = _fileProvider.GetFileExtension(fileName);
        if (!string.IsNullOrEmpty(fileExtension))
            fileExtension = fileExtension.ToLowerInvariant();

        if (imgExt.All(ext => !ext.Equals(fileExtension, StringComparison.CurrentCultureIgnoreCase)))
            return null;

        //contentType is not always available 
        //that's why we manually update it here
        //https://mimetype.io/all-types/
        if (string.IsNullOrEmpty(contentType))
            contentType = GetPictureContentTypeByFileExtension(fileExtension);

        if (contentType == MimeTypes.ImageSvg && !_mediaSettings.AllowSVGUploads)
            return null;

        var picture = await InsertPictureAsync(await _downloadService.GetDownloadBitsAsync(formFile),
            contentType,
            _fileProvider.GetFileNameWithoutExtension(fileName),
            validateBinary: contentType != MimeTypes.ImageSvg);

        if (string.IsNullOrEmpty(virtualPath))
            return picture;

        picture.VirtualPath = _fileProvider.GetVirtualPath(virtualPath);
        await UpdatePictureAsync(picture);

        return picture;
    }
    #endregion
}
