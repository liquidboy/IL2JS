using System;
using System.Collections.Generic;
using Microsoft.LiveLabs.Html;
using Microsoft.LiveLabs.JavaScript.IL2JS;
using Microsoft.LiveLabs.JavaScript.Interop;
using Microsoft.Csa.EventLink.Client;
using Microsoft.Csa.EventLink;
using Microsoft.Csa.SharedObjects.Utilities;
using Microsoft.LiveLabs.Xml;
using System.Net;
using System.Net.Browser;
using Microsoft.LiveLabs.JavaScript;
using Microsoft.Csa.SharedObjects;
using System.Linq;
using System.IO;
using Microsoft.Csa.SharedObjects.Client;

namespace EventLink.Test.Javascript
{
    public class EventLinkPage : Page
    {
        // Entry point for JavaScript target (boilerplate)
        [EntryPoint]
        public static void Run()
        {
            new EventLinkPage();
        }
        
        private EventLinkChannel Channel { get; set; }
        private EventLinkClient client;
        

        public class SampleEntry : ISharedObjectEntry
        {

            #region ISharedObjectEntry Members

            public SharedObjectSecurity ObjectSecurity
            {
                get
                {
                    return new SharedObjectSecurity();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public Guid Id
            {
                get { return Guid.Empty; }
            }

            public string Name
            {
                get { return "SampleEntry"; }
            }

            #endregion
        }

        private void OnIncomingChannelsInitialized()
        {
            string clientSubscriptionId = this.Channel.ChannelName;
            Console.WriteLine("OnIncomingChannelsInitialized");

            // TODO: the ClientId could probably just be the clientSubscriptionId

            //ClientConnectPayload payload = new ClientConnectPayload(clientSubscriptionId, this.ClientId, this.Namespace, this.PrincipalObject);
            //this.Channel.SendEvent(this.globalListeningChannelName, payload);
        }

        private void OnIncomingChannelsDataReceived(IEnumerable<Payload> payloads)
        {
            Console.WriteLine("OnIncomingChannelsDataReceived");
        }

        public string WriteToJson(ISharedObjectSerializable obj)
        {
            using (var stream = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(stream);
                using (JsonPayloadWriter writer = new JsonPayloadWriter(sw))
                {
                    writer.Write(string.Empty, obj);
                }
                stream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }

        public string WriteToJson(EventSet[] eventsets)
        {
            using (var stream = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(stream);
                using (JsonPayloadWriter writer = new JsonPayloadWriter(sw))
                {
                    writer.Write(string.Empty, eventsets);
                }
                stream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }

        private Stream StreamFromString(string input)
        {
            var stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream);
            sw.Write(input);
            sw.Flush();
            return stream;
        }

        public EventLinkPage()
        {
            var text = new Div() { Text = "Server Address:" };
            var textbox = new Input() { Value="elipc:4040" };
            var div = new Div();
            div.Add(text);
            div.Add(textbox);
            Browser.Document.Body.Add(div);

            // Conventional DOM interaction
            var jsonButton = new Button { InnerHtml = "Test JSON..." };
            jsonButton.Click += e =>
            {
                MemoryStream st = new MemoryStream();
                PrincipalObject obj = new PrincipalObject() { Sid = "sa", Id = "eli" };
                Console.WriteLine(obj);

                JsonPayloadWriter w = new JsonPayloadWriter(st);
                var pl = new ObjectPayload(Guid.NewGuid(), obj, Guid.NewGuid(), "ObjectName");
                Console.WriteLine(pl);
                w.Write(string.Empty, pl);
                StreamReader sr = new StreamReader(st);
                string output = sr.ReadToEnd();
                Console.WriteLine(output);
                return;



                //DUMMY TEST
                //string jsont = "{ \"ChannelName\": \"test\" }";
                //JsonPayloadReader jsr = new JsonPayloadReader(jsont);
                //Console.WriteLine(jsr.ReadString("ChannelName"));

                string ns = "namespace1";
                string subscriptionId = "1234";
                Guid clientId = Guid.NewGuid();
                Guid parentId = Guid.NewGuid();
                Guid objectId = Guid.NewGuid();

                Payload payload = new ClientConnectPayload(subscriptionId, clientId, ns, NamespaceLifetime.ServerInstance);
                ETag eTag = new ETag(clientId, 2);
                string json = WriteToJson(payload);
                Console.WriteLine("WROTE: {0}", json);

                JsonPayloadReader jsonReader = new JsonPayloadReader(json);
                var payloadResult = jsonReader.ReadObject<Payload>(string.Empty, Payload.CreateInstance);
                Console.WriteLine("READ OBJECT: {0}", payloadResult);

                EventSet[] events = new EventSet[]
                {
                    new EventSet { Sequence = 0, ChannelName = "test", Payloads = new Payload[] { new ClientConnectPayload(subscriptionId, clientId , ns, NamespaceLifetime.ConnectedOnly) } }
                };

                json = WriteToJson(events);
                Console.WriteLine(json);

                MemoryStream ms = new MemoryStream();
                StreamWriter sw = new StreamWriter(ms);
                sw.Write(json);
                sw.Flush();

                var eventsResult = EventSet.CreateEventSetsFromStream(ms, PayloadFormat.JSON);
                Console.WriteLine(eventsResult);

                var principal = new PrincipalObject() { Id = "Eli", Sid = "sa" };
                var es = new EventSet { Sequence = 0, ChannelName = "test", Payloads = new Payload[] { new ClientConnectPayload(subscriptionId, clientId, ns, NamespaceLifetime.ConnectedOnly, principal) } };

                json = WriteToJson(es);
                Console.Write(json);

                return;              
            };
            Browser.Document.Body.Add(jsonButton);
            
            //var soButton = new Button { InnerHtml = "Subscribe Shared Objects Client" };
            //soButton.Click += e =>
            //{
            //    var guid = Guid.NewGuid();
            //    var txt = textbox.Value;

            //    string path = string.Format("http://{0}/{1}", txt, guid.ToString());

            //    SharedObjectsClient soc = new SharedObjectsClient(path, NamespaceLifetime.ConnectedOnly);
            //    PrincipalObject principal = new PrincipalObject() { Id = "ClientA", Sid = "sa" };
            //    soc.PrincipalObject = principal;
                
            //    soc.Connect();
            //    Console.WriteLine("Connecting to:{0}", path);
            //    soc.Connected += (s, ec) =>
            //    {
            //        Console.WriteLine("CONNECTED TO SHARED OBJECTS SERVER");
            //    };
            //};
            //Browser.Document.Body.Add(soButton);

            //// Conventional DOM interaction
            //var subButton = new Button { InnerHtml = "Subscribe..." };
            //subButton.Click += e =>
            //{
            //    Console.WriteLine("Subscribing...");

            //    Uri uri = new Uri("http://elipc:4040/");
            //    this.client = new EventLinkClient(uri, "3bb04637-af98-40e9-ad65-64fb2668a0d2", (s, ex) =>
            //    {
            //        Console.WriteLine(string.Format("Error state:{0} exception:{1}", s, ex));
            //    });
            //    this.client.Subscribe("99ca2aff-538e-49f2-a71c-79b7720e3f21ClientControl", EventLinkEventsReceived, this.OnSubscriptionInitialized);
            //    return;

            //    //this.Channel = new EventLinkChannel(new Uri("http://elipc:4040/"), "nameSpace", (c1, c2) =>
            //    //{
            //    //    Write("Error state changed");
            //    //});

            //    //this.Channel.SubscriptionInitialized += OnIncomingChannelsInitialized;
            //    //this.Channel.EventsReceived += OnIncomingChannelsDataReceived;
            //    //string name = Channels.GetClientName(Guid.NewGuid());
            //    //this.Channel.SubscribeAsync(name);

            //    //Uri uri = new Uri("http://elipc:4040/");
            //    //this.client = new EventLinkClient(uri, "f148dc15-ad26-484e-9f74-8d0655e8fc0d", (s, ex) =>
            //    //{
            //    //    Console.WriteLine(string.Format("Error state:{0} exception:{1}", s, ex));
            //    //});
            //    //this.client.Subscribe("81dddf4c-f84c-414f-9f04-99f926fc8c68ClientControl", EventLinkEventsReceived, () => this.OnSubscriptionInitialized());
            //};
            //Browser.Document.Body.Add(subButton);

            //// Conventional DOM interaction
            //var secButton = new Button { InnerHtml = "Test Security..." };
            //secButton.Click += e =>
            //{
            //    Console.WriteLine("sec");

            //    //var el = new EventLinkClient(new Uri("http://localhost:4040/"), "partitionA", (c) =>
            //    //{
            //    //    Write("EventLinkClient Connected");
            //    //});

            //    //var att = new SharedAttributes();
            //    //Write(att);

            //    //var client = new SharedObjectsClient("http://www.cnn.com/namespaceName");
            //    //Write(client);

            //    //client.Connect();

            //    Guid g = Guid.NewGuid();
            //    Console.WriteLine(g);

            //    Guid g2 = Guid.Empty;
            //    Console.WriteLine(g2);

            //    ETag e2 = new ETag(Guid.Empty);
            //    Console.WriteLine(e2);

            //    SharedObjectSecurity s = new SharedObjectSecurity("Security", false, new ETag(Guid.Empty));
            //    s.AddAccessRule(new SharedObjectAccessRule("Idenity", ObjectRights.ChangePermissions, System.Security.AccessControl.InheritanceFlags.ContainerInherit, System.Security.AccessControl.AccessControlType.Allow));
            //    Console.WriteLine(s);

            //    PrincipalObject p = new PrincipalObject() { Id = "Hello", Sid = "Sid" };
            //    Console.WriteLine(p);

            //    SharedEntryMap<SampleEntry> map = new SharedEntryMap<SampleEntry>();
            //    Console.WriteLine(map);

            //    var attr = new SharedAttributes();
            //    Console.WriteLine(attr);

            //    var ep = new ObjectExpirationPolicy("timespan", TimeSpan.FromDays(1), TimeSpan.FromMilliseconds(100));
            //    Console.WriteLine(ep);

            //    var v = new ProtocolVersion(1, 0);
            //    Console.WriteLine(v);

            //    //var tp = new TracePayload("Message", Guid.Empty);
            //    //Write(tp);


                
            //    //ObjectDeletedPayload p2 = new ObjectDeletedPayload(Guid.Empty, Guid.NewGuid());
            //    //Write(p2);

            //    ////WAIT FOR SAHRED PROEPRPT
            //    ////ClientConnectPayload c = new ClientConnectPayload("sub", Guid.Empty, "namespace");
            //    ////Write(c);



            //    //var principalObject = new PrincipalObject() { Id = "Eli", Sid = "sa" };
            //    ////                var principal = new ObjectPayload(Guid.NewGuid(), principalObject, Guid.NewGuid(), "NamedObject");

            //    //var sharedProps = new Dictionary<short, SharedProperty>();
            //    //Write(sharedProps);

            //    ////TODO THIS IS BROKEN 
            //    ////var spd = new SharedPropertyDictionary();
            //    ////Write(spd);
                
            //    //var puo = new PropertyUpdateOperation();
            //    //Write(puo);



            //    //JavascriptHttpStream str = new JavascriptHttpStream(null);
            //    //BinaryReader br = new BinaryReader(str);
            //    //BinaryWriter bw = new BinaryWriter(str);
            //    //BinaryReader br = new BinaryReader(null);
            //    //BinaryWriter bw = new BinaryWriter(null);

                
                
            //};
            ////Browser.Document.Body.Add(secButton);

            // Conventional DOM interaction
            var linqButton = new Button { InnerHtml = "Test Linq..." };
            linqButton.Click += e =>
            {
                int value = "abc".Select(a => a - 'a').Sum();
                Console.WriteLine(value);

                value = "abcdefg".Select(a => a - 'a').Sum();
                Console.WriteLine(value);
            };

            //Browser.Document.Body.Add(linqButton);
        }

        protected void OnSubscriptionInitialized()
        {
            Console.WriteLine("EventLinkPage.OnSubscriptionInitialized");
            var button = new Button { InnerText = "Publish..." };
            button.Click += e =>
            {
               client.Publish("97fd0fd1-d1f2-4987-962e-2faad0cb842f", new Payload[] { null, null });
                //JsonPayload pay = new JsonPayload() 
                //{
                //    
                //};

                //client.Publish("97fd0fd1-d1f2-4987-962e-2faad0cb842f", new Payload[] { pay });
            };
            Browser.Document.Body.Add(button);
        }

        private void EventLinkEventsReceived(EventSet[] eventSets)
        {
            if (eventSets == null)
            {
                return;
            }

            Console.WriteLine("EventReceived");
        }
    }
}
