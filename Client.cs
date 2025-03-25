namespace UnsendSDK
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using static UnsendSDK.UnsendClient;

    public class UnsendClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        public Email emailService;
        public Contacts contactsService;
        public Domains domainsService;
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsendClient"/> class.
        /// </summary>
        /// <param name="apiKey">Your API key for authentication with Unsend.</param>
        /// <param name="baseUrl">The base URL of the Unsend API. Default is "https://app.unsend.dev/".</param>
        public UnsendClient(string apiKey, string baseUrl = "https://app.unsend.dev/")
        {
            _apiKey = apiKey;
            _baseUrl = baseUrl;
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            emailService = new Email(apiKey, baseUrl);
            contactsService = new Contacts(apiKey, baseUrl);
            domainsService = new Domains(apiKey, baseUrl);
        }
        /// <summary>
        /// Client for interacting with the Unsend API Email service.
        /// </summary>
        public class Email
        {
            private readonly HttpClient _httpClient;
            private readonly string _apiKey;
            private readonly string _baseUrl;
            /// <summary>
            /// Initializes a new instance of the <see cref="Email"/> class.
            /// </summary>
            /// <param name="apiKey">Your API key for authentication with Unsend.</param>
            /// <param name="baseUrl">The base URL of the Unsend API. Default is "https://app.unsend.dev/".</param>
            public Email(string apiKey, string baseUrl = "https://app.unsend.dev/")
            {
                _apiKey = apiKey;
                _baseUrl = baseUrl;
                _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            }
            /// <summary>
            /// Sends an email using the Unsend API with a scheduled time.
            /// </summary>
            /// <param name="to">Recipient's email address.</param>
            /// <param name="subject">Subject of the email.</param>
            /// <param name="from">Sender's email address.</param>
            /// <param name="scheduledAt">Date and time when the email should be sent.</param>
            /// <param name="templateId">Optional template ID for the email.</param>
            /// <param name="replyTo">Optional reply-to email address.</param>
            /// <param name="cc">Optional CC email addresses.</param>
            /// <param name="bcc">Optional BCC email addresses.</param>
            /// <param name="text">Optional plain text content of the email.</param>
            /// <param name="html">Optional HTML content of the email.</param>
            /// <param name="attachments">Optional list of attachments.</param>
            public async Task<EmailId> SendEmailAsync(List<string> to, string subject, string from, DateTime scheduledAt, string templateId = "", string replyTo = "", List<string> cc = null, List<string> bcc = null, string text = "", string html = "", List<Attachment> attachments = null)
            {
                if (cc == null)
                {
                    cc = new List<string>();
                }
                if (bcc == null)
                {
                    bcc = new List<string>();
                }
                if (attachments == null)
                {
                    attachments = new List<Attachment>();
                }
                UnsendSendMail sendMail = new UnsendSendMail
                {
                    to = to,
                    from = from,
                    subject = subject,
                    templateId = templateId,
                    replyTo = replyTo,
                    cc = cc,
                    bcc = bcc,
                    text = text,
                    html = html,
                    attachments = attachments,
                    scheduledAt = scheduledAt.ToString("yyyy-MM-ddTHH:mm:ssZ")
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/emails");
                var content = new StringContent(JsonConvert.SerializeObject(sendMail, Formatting.Indented), null, "application/json");
                request.Content = content;
                var response = await _httpClient.SendAsync(request);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                string resposta = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EmailId>(await response.Content.ReadAsStringAsync());
            }
            /// <summary>
            /// Sends an email using the Unsend API without a scheduled time. (it will be sended with DateTime.Now.AddSeconds(5))
            /// </summary>
            /// <param name="to">Recipient's email address.</param>
            /// <param name="subject">Subject of the email.</param>
            /// <param name="from">Sender's email address.</param>            
            /// <param name="templateId">Optional template ID for the email.</param>
            /// <param name="replyTo">Optional reply-to email address.</param>
            /// <param name="cc">Optional CC email addresses.</param>
            /// <param name="bcc">Optional BCC email addresses.</param>
            /// <param name="text">Optional plain text content of the email. But mandatory if html is not sended</param>
            /// <param name="html">Optional HTML content of the email. But mandatory if text is not sended</param>
            /// <param name="attachments">Optional list of attachments.</param>
            public async Task<EmailId> SendEmailAsync(List<string> to, string subject, string from, string templateId = "", string replyTo = "", List<string> cc = null, List<string> bcc = null, string text = "", string html = "", List<Attachment> attachments = null)
            {
                if (cc == null)
                {
                    cc = new List<string>();
                }
                if (bcc == null)
                {
                    bcc = new List<string>();
                }
                if (attachments == null)
                {
                    attachments = new List<Attachment>();
                }
                UnsendSendMail sendMail = new UnsendSendMail
                {
                    to = to,
                    from = from,
                    subject = subject,
                    templateId = templateId,
                    replyTo = replyTo,
                    cc = cc,
                    bcc = bcc,
                    text = text,
                    html = html,
                    attachments = attachments,
                    scheduledAt = DateTime.Now.AddSeconds(5).ToString("yyyy-MM-ddTHH:mm:ssZ")
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/emails");
                var content = new StringContent(JsonConvert.SerializeObject(sendMail, Formatting.Indented), null, "application/json");
                request.Content = content;
                var response = await _httpClient.SendAsync(request);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                string resposta = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EmailId>(await response.Content.ReadAsStringAsync());
            }
            /// <summary>
            /// Get an email using the Unsend API
            /// </summary>
            /// <param name="emailId">The emailid that you want to get information.</param>
            public async Task<EmailData> GetEmailAsync(string emailId)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/emails/{emailId}");
                var response = await _httpClient.SendAsync(request);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                string resposta = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EmailData>(await response.Content.ReadAsStringAsync());
            }
            /// <summary>
            /// Update an email schedule using the Unsend API
            /// </summary>
            /// <param name="emailId">The emailid that you want to update the schedule.</param>
            /// <param name="scheduledAt">The new DateTime that you want to pass to the server.</param>
            public async Task<EmailId> UpdateScheduleAsync(string emailId, DateTime scheduledAt)
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/emails/{emailId}");
                var content = new StringContent($"{{\r\n  \"scheduledAt\": \"{scheduledAt.ToString("yyyy-MM-ddTHH:mm:ssZ")}\"\r\n}}", null, "application/json");
                request.Content = content;
                var response = await _httpClient.SendAsync(request);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                string resposta = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EmailId>(await response.Content.ReadAsStringAsync());
            }
            /// <summary>
            /// Cancel an email schedule using the Unsend API
            /// </summary>
            /// <param name="emailId">The emailid that you want to cancel the schedule.</param>
            public async Task<EmailId> CancelScheduleAsync(string emailId)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/emails/{emailId}/cancel");
                var response = await _httpClient.SendAsync(request);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                string resposta = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EmailId>(await response.Content.ReadAsStringAsync());

            }

            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            public class EventData
            {
                public DateTime? timestamp { get; set; }
                public List<string> recipients { get; set; }
                public string remoteMtaIp { get; set; }
                public string reportingMTA { get; set; }
                public string smtpResponse { get; set; }
                public int? processingTimeMillis { get; set; }
            }

            public class EmailEvent
            {
                public string emailId { get; set; }
                public string status { get; set; }
                public DateTime createdAt { get; set; }
                public EventData data { get; set; }
            }

            public class EmailData
            {
                public string id { get; set; }
                public int teamId { get; set; }
                public List<string> to { get; set; }
                public string from { get; set; }
                public string subject { get; set; }
                public string html { get; set; }
                public string text { get; set; }
                public DateTime createdAt { get; set; }
                public DateTime updatedAt { get; set; }
                public List<EmailEvent> emailEvents { get; set; }
            }

            public class EmailId
            {
                public string emailId { get; set; }
            }

        }
        /// <summary>
        /// Client for interacting with the Unsend API Contacts.
        /// </summary>
        public class Contacts
        {
            private readonly HttpClient _httpClient;
            private readonly string _apiKey;
            private readonly string _baseUrl;

            public Contacts(string apiKey, string baseUrl = "https://app.unsend.dev/")
            {
                _apiKey = apiKey;
                _baseUrl = baseUrl;
                _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            }


            public async Task<ContactInfo> CreateContactAsync(string email, string firstName, string lastName, string contactBookId, bool subscribed)
            {
                ContactInfo newContact = new ContactInfo
                {
                    email = email,
                    firstName = firstName,
                    lastName = lastName,
                    subscribed = subscribed
                };
                var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/contactBooks/{contactBookId}/contacts");
                var content = new StringContent(JsonConvert.SerializeObject(newContact, Formatting.Indented), null, "application/json");
                request.Content = content;
                var response = await _httpClient.SendAsync(request);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                ContactInfo info = JsonConvert.DeserializeObject<ContactInfo>(await response.Content.ReadAsStringAsync());
                newContact.contactId = info.contactId;
                return response.StatusCode == System.Net.HttpStatusCode.OK ? newContact : null;

            }

            public async Task<ContactInfo> UpdateContactAsync(string firstName, string lastName, string contactBookId, string contactId, bool subscribed)
            {
                ContactInfo newContact = new ContactInfo
                {
                    firstName = firstName,
                    lastName = lastName,
                    subscribed = subscribed
                };
                var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/contactBooks/{contactBookId}/contacts/{contactId}");
                var content = new StringContent(JsonConvert.SerializeObject(newContact, Formatting.Indented), null, "application/json");
                request.Content = content;
                var response = await _httpClient.SendAsync(request);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                ContactInfo info = JsonConvert.DeserializeObject<ContactInfo>(await response.Content.ReadAsStringAsync());
                newContact.contactId = info.contactId;
                return response.StatusCode == System.Net.HttpStatusCode.OK ? newContact : null;

            }
            public async Task<ContactInfo> UpsertContactAsync(string email, string firstName, string lastName, string contactBookId, string contactId, bool subscribed)
            {
                ContactInfo newContact = new ContactInfo
                {
                    email = email,
                    firstName = firstName,
                    lastName = lastName,
                    subscribed = subscribed
                };
                var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/contactBooks/{contactBookId}/contacts/{contactId}");
                var content = new StringContent(JsonConvert.SerializeObject(newContact, Formatting.Indented), null, "application/json");
                request.Content = content;
                var response = await _httpClient.SendAsync(request);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                ContactInfo info = JsonConvert.DeserializeObject<ContactInfo>(await response.Content.ReadAsStringAsync());
                newContact.contactId = info.contactId;
                return response.StatusCode == System.Net.HttpStatusCode.OK ? newContact : null;

            }

            public async Task<bool> DeleteContactAsync(string contactBookId, string contactId)
            {

                var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/contactBooks/{contactBookId}/contacts/{contactId}");
                var response = await _httpClient.SendAsync(request);
                return response.StatusCode == System.Net.HttpStatusCode.OK;

            }

            public async Task<ContactInfo> GetContactAsync(string contactBookId, string contactId)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/contactBooks/{contactBookId}/contacts/{contactId}");
                var response = await _httpClient.SendAsync(request);
                return JsonConvert.DeserializeObject<ContactInfo>(await response.Content.ReadAsStringAsync());

            }

            public class ContactInfo
            {
                public string contactBookId { get; set; }
                public string contactId { get; set; }
                public string email { get; set; }
                public string firstName { get; set; }
                public string lastName { get; set; }
                public bool subscribed { get; set; }
            }


        }
        /// <summary>
        /// Client for interacting with the Unsend API Domains.
        /// </summary>
        public class Domains
        {
            private readonly HttpClient _httpClient;
            private readonly string _apiKey;
            private readonly string _baseUrl;
            public Domains(string apiKey, string baseUrl = "https://app.unsend.dev/")
            {
                _apiKey = apiKey;
                _baseUrl = baseUrl;
                _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            }
            /// <summary>
            /// Get domains information
            /// </summary>
            public async Task<List<DomainData>> GetDomains()
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/v1/domains");
                var response = await _httpClient.SendAsync(request);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                return JsonConvert.DeserializeObject<List<DomainData>>(await response.Content.ReadAsStringAsync());
            }

            public class DomainData
            {
                public int id { get; set; }
                public string name { get; set; }
                public int teamId { get; set; }
                public string status { get; set; }
                public string region { get; set; }
                public bool clickTracking { get; set; }
                public bool openTracking { get; set; }
                public string publicKey { get; set; }
                public string dkimStatus { get; set; }
                public string spfDetails { get; set; }
                public string createdAt { get; set; }
                public string updatedAt { get; set; }
            }
        }
    }

    /// <summary>
    /// Represents an email attachment.
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Filename of the attachment.
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// Base64-encoded content of the attachment.
        /// </summary>
        public string content { get; set; }
    }

    internal class UnsendSendMail
    {
        public List<string> to { get; set; }
        public string from { get; set; }
        public string subject { get; set; }
        public string templateId { get; set; }
        public string replyTo { get; set; }
        public List<string> cc { get; set; }
        public List<string> bcc { get; set; }
        public string text { get; set; }
        public string html { get; set; }
        public List<Attachment> attachments { get; set; }
        public string scheduledAt { get; set; }
    }

    internal class Variables
    {
    }



}
