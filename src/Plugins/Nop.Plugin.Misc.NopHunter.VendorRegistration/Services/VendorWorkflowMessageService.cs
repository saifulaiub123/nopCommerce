using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core;
using Nop.Core.Domain.Vendors;
using Nop.Core.Events;
using Nop.Services.Affiliates;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Stores;
using Nop.Core.Domain.Common;
using Nop.Plugin.Misc.VendorRegistration;
using Nop.Core.Domain.Logging;
using Nop.Data;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Services;
public class VendorWorkflowMessageService : WorkflowMessageService, IVendorWorkflowMessageService
{
    #region Field
    protected readonly CommonSettings _commonSettings;
    protected readonly EmailAccountSettings _emailAccountSettings;
    protected readonly IAddressService _addressService;
    protected readonly IAffiliateService _affiliateService;
    protected readonly ICustomerService _customerService;
    protected readonly IEmailAccountService _emailAccountService;
    protected readonly IEventPublisher _eventPublisher;
    protected readonly ILanguageService _languageService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IMessageTemplateService _messageTemplateService;
    protected readonly IMessageTokenProvider _messageTokenProvider;
    protected readonly IOrderService _orderService;
    protected readonly IProductService _productService;
    protected readonly IQueuedEmailService _queuedEmailService;
    protected readonly IStoreContext _storeContext;
    protected readonly IStoreService _storeService;
    protected readonly ITokenizer _tokenizer;
    protected readonly IWorkflowMessageService _workflowMessageService;
    protected readonly MessagesSettings _messagesSettings;
    protected readonly IRepository<Log> _logRepository;


    public VendorWorkflowMessageService(
        CommonSettings commonSettings,
        EmailAccountSettings emailAccountSettings,
        IAddressService addressService,
        IAffiliateService affiliateService,
        ICustomerService customerService,
        IEmailAccountService emailAccountService,
        IEventPublisher eventPublisher,
        ILanguageService languageService,
        ILocalizationService localizationService,
        IMessageTemplateService messageTemplateService,
        IMessageTokenProvider messageTokenProvider,
        IOrderService orderService,
        IProductService productService,
        IQueuedEmailService queuedEmailService,
        IStoreContext storeContext,
        IStoreService storeService,
        ITokenizer tokenizer,
        IWorkflowMessageService workflowMessageService,
        MessagesSettings messagesSettings,
        IRepository<Log> logRepository
        ) : base(
                commonSettings,
                emailAccountSettings,
                addressService,
                affiliateService,
                customerService,
                emailAccountService,
                eventPublisher,
                languageService,
                localizationService,
                messageTemplateService,
                messageTokenProvider,
                orderService,
                productService,
                queuedEmailService,
                storeContext,
                storeService,
                tokenizer,
                messagesSettings
            )
    {
        _commonSettings = commonSettings;
        _emailAccountSettings = emailAccountSettings;
        _addressService = addressService;
        _affiliateService = affiliateService;
        _customerService = customerService;
        _emailAccountService = emailAccountService;
        _eventPublisher = eventPublisher;
        _languageService = languageService;
        _localizationService = localizationService;
        _messageTemplateService = messageTemplateService;
        _messageTokenProvider = messageTokenProvider;
        _orderService = orderService;
        _productService = productService;
        _queuedEmailService = queuedEmailService;
        _storeContext = storeContext;
        _storeService = storeService;
        _tokenizer = tokenizer;
        _messagesSettings = messagesSettings;
        _workflowMessageService = workflowMessageService;
        _logRepository = logRepository;
    }
    #endregion

    public async Task<IList<int>> SendVendorAccountCreationNotificationToVendor(Customer customer, Vendor vendor, int languageId)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        var store = await _storeContext.GetCurrentStoreAsync();
        languageId = await EnsureLanguageIsActiveAsync(languageId, store.Id);

        var messageTemplates = await GetActiveMessageTemplatesAsync(VendorRegistrationDefaults.NEW_VENDOR_ACCOUNT_APPLY_STORE_VENDOR_NOTIFICATION, store.Id);
        if (!messageTemplates.Any())
        {
            //TODO: Add log to db that message template not found. 
            return new List<int>();
        }
            

        //tokens
        var commonTokens = new List<Token>();
        await _messageTokenProvider.AddVendorTokensAsync(commonTokens, vendor);

        return await messageTemplates.SelectAwait(async messageTemplate =>
        {
            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate, languageId);

            var tokens = new List<Token>(commonTokens);
            await _messageTokenProvider.AddStoreTokensAsync(tokens, store, emailAccount, languageId);

            //event notification
            await _eventPublisher.MessageTokensAddedAsync(messageTemplate, tokens);

            var toEmail = vendor.Email;
            var toName = $"{customer.FirstName} {customer.LastName}";


            var vendorAddress = await _addressService.GetAddressByIdAsync(vendor.AddressId);
            //var replyToEmail = messageTemplate.AllowDirectReply ? vendorAddress.Email : "";
            //var replyToName = messageTemplate.AllowDirectReply ? $"{vendorAddress.FirstName} {vendorAddress.LastName}" : "";

            return await SendNotificationAsync(messageTemplate, emailAccount, languageId, tokens, toEmail, toName, replyToEmailAddress: string.Empty, replyToName: string.Empty);
        }).ToListAsync();
    }
}
