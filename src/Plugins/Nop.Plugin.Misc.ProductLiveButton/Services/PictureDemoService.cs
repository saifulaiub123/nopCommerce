using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Nop.Plugin.Misc.ProductLiveButton.Services;
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

        pictureBinary.BinaryData = binaryData;

        if (isNew)
            await _pictureBinaryRepository.InsertAsync(pictureBinary);
        else
            await _pictureBinaryRepository.UpdateAsync(pictureBinary);

        return pictureBinary;
    }
    #endregion
}
