using System; 
using System.Linq; 
using Google.Apis.Compute.v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Cloud.Storage.V1;
using System.Configuration; 
using log4net;
using Google.Apis.DeploymentManager.v2;
using System.IO;
using ComputeServiceData = Google.Apis.Compute.v1.Data;
using DeploymentManagerData = Google.Apis.DeploymentManager.v2.Data;
using SQLAdmin = Google.Apis.SQLAdmin.v1beta4;
using SQLAdminData = Google.Apis.SQLAdmin.v1beta4.Data;

namespace GetGCPResourceClient
{
    public class GetResources
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Snaps the Routes.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapRoutes(Subscription subscription, StreamWriter writer)
        {
            writer.Write("Routes");
            writer.Write(Environment.NewLine);
            writer.Write("Route Name", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            RoutesResource.ListRequest request = computeService.Routes.List(project);
            ComputeServiceData.RouteList response = null;
            do
            {
                try
                {
                    response = request.Execute();
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Exception while executing RouteList request: {0}", ex.ToString());
                }
                if (response.Items == null)
                {
                    continue;
                }

                foreach (ComputeServiceData.Route route in response.Items)
                {
                    if (route.NextHopGateway != null)
                    {
                        writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", route.Name, subscription.SubscriptionFriendlyName, route.CreationTimestamp, ""));
                        writer.Write(Environment.NewLine);
                    }
                }
            } while (response.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the GlobalForwardingRules.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapGlobalForwardingRules(Subscription subscription, StreamWriter writer)
        {
            writer.Write("GlobalForwardingRules");
            writer.Write(Environment.NewLine);
            writer.Write("ForwardingRules Name", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            GlobalForwardingRulesResource.ListRequest forwardRuleRequest = computeService.GlobalForwardingRules.List(project);
            ComputeServiceData.ForwardingRuleList forwardRuleResponse = null;
            do
            {
                try
                {
                    forwardRuleResponse = forwardRuleRequest.Execute();
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Exception while executing GlobalForwardingRulesResource request: {0}", ex.ToString());
                }
                if (forwardRuleResponse.Items == null)
                {
                    continue;
                }

                foreach (ComputeServiceData.ForwardingRule forwardingRule in forwardRuleResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", forwardingRule.Name, subscription.SubscriptionFriendlyName, forwardingRule.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (forwardRuleResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the GlobalForwardingRules.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapForwardingRules(Subscription subscription, StreamWriter writer, string RegionName)
        {
            writer.Write("ForwardingRules");
            writer.Write(Environment.NewLine);
            writer.Write("ForwardingRules Name", "Region", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            ForwardingRulesResource.ListRequest forwardingRulesRequest = computeService.ForwardingRules.List(project, RegionName);
            ComputeServiceData.ForwardingRuleList forwardingRulesResponse = null;
            do
            {
                try
                {
                    forwardingRulesResponse = forwardingRulesRequest.Execute();
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Exception while executing ForwardingRulesResource request: {0}", ex.ToString());
                }
                if (forwardingRulesResponse.Items == null)
                {
                    continue;
                }

                foreach (ComputeServiceData.ForwardingRule forwardingRule in forwardingRulesResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", forwardingRule.Name, RegionName, subscription.SubscriptionFriendlyName, forwardingRule.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (forwardingRulesResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the TargetPool.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapTargetPool(Subscription subscription, StreamWriter writer, string RegionName)
        {
            writer.Write("TargetPool");
            writer.Write(Environment.NewLine);
            writer.Write("TargetPool Name", "Region", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            TargetPoolsResource.ListRequest targetPoolsRequest = computeService.TargetPools.List(project, RegionName);
            ComputeServiceData.TargetPoolList targetPoolsResponse = null;
            do
            {
                try
                {
                    targetPoolsResponse = targetPoolsRequest.Execute();
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Exception while executing TargetPools request: {0}", ex.ToString());
                }
                if (targetPoolsResponse.Items == null)
                {
                    continue;
                }

                foreach (ComputeServiceData.TargetPool targetPool in targetPoolsResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", targetPool.Name, RegionName, subscription.SubscriptionFriendlyName, targetPool.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (targetPoolsResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Deletes the target HTTP proxy.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        private static StreamWriter SnapTargetHttpProxy(Subscription subscription, StreamWriter writer)
        {
            writer.Write("TargetPool");
            writer.Write(Environment.NewLine);
            writer.Write("TargetPool Name", "Region", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;

            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            ComputeServiceData.TargetHttpProxyList targetHttpProxyResponse = null;
            do
            {
                try
                {
                    targetHttpProxyResponse = computeService.TargetHttpProxies.List(project).Execute();
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Exception while executing TargetPools request: {0}", ex.ToString());
                }

                if (targetHttpProxyResponse.Items == null)
                {
                    continue;
                }

                foreach (var targetHttpProxy in targetHttpProxyResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", targetHttpProxy.Name, subscription.SubscriptionFriendlyName, targetHttpProxy.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            }
            while (targetHttpProxyResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the UrlMaps.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapUrlMaps(Subscription subscription, StreamWriter writer)
        {
            writer.Write("UrlMaps");
            writer.Write(Environment.NewLine);
            writer.Write("UrlMaps Name", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            ComputeServiceData.UrlMapList response = null;
            do
            {
                try
                {
                    response = computeService.UrlMaps.List(project).Execute();
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Exception while executing UrlMaps request: {0}", ex.ToString());
                }
                if (response.Items == null)
                {
                    continue;
                }

                foreach (var urlMap in response.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", urlMap.Name, subscription.SubscriptionFriendlyName, urlMap.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (response.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the RegionBackendServices.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapRegionBackendServices(Subscription subscription, StreamWriter writer, string RegionName)
        {
            writer.Write("RegionBackendServices");
            writer.Write(Environment.NewLine);
            writer.Write("RegionBackendServices Name", "Region", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            RegionBackendServicesResource.ListRequest regionBackendServiceRequest = computeService.RegionBackendServices.List(project, RegionName);
            ComputeServiceData.BackendServiceList regionBackendServiceResponse;
            do
            {
                regionBackendServiceResponse = regionBackendServiceRequest.Execute();
                if (regionBackendServiceResponse.Items == null)
                {
                    continue;
                }

                foreach (ComputeServiceData.BackendService regionBackendSerive in regionBackendServiceResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", regionBackendSerive.Name, RegionName, subscription.SubscriptionFriendlyName, regionBackendSerive.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (regionBackendServiceResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the BackendServices.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapBackendServices(Subscription subscription, StreamWriter writer)
        {
            writer.Write("BackendServices");
            writer.Write(Environment.NewLine);
            writer.Write("BackendServices Name", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            BackendServicesResource.ListRequest backendServiceRequest = computeService.BackendServices.List(project);
            ComputeServiceData.BackendServiceList backendServiceRequestResponse;
            do
            {
                backendServiceRequestResponse = backendServiceRequest.Execute();
                if (backendServiceRequestResponse.Items == null)
                {
                    continue;
                }

                foreach (ComputeServiceData.BackendService backendService in backendServiceRequestResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", backendService.Name, subscription.SubscriptionFriendlyName, backendService.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (backendServiceRequestResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the BackendBuckets.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapBackendBuckets(Subscription subscription, StreamWriter writer)
        {
            writer.Write("BackendBuckets");
            writer.Write(Environment.NewLine);
            writer.Write("BackendBuckets Name", "Bucket Name", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            BackendBucketsResource.ListRequest backendBucketRequest = computeService.BackendBuckets.List(project);
            ComputeServiceData.BackendBucketList backendBucketResponse = backendBucketRequest.Execute();
            do
            {
                if (backendBucketResponse.Items == null)
                {
                    continue;
                }

                foreach (var backendBucket in backendBucketResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", backendBucket.Name, backendBucket.BucketName, subscription.SubscriptionFriendlyName, backendBucket.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (backendBucketResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the InstanceGroupManagers.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapInstanceGroupManagers(Subscription subscription, StreamWriter writer, string ZoneName)
        {
            writer.Write("InstanceGroupManagers");
            writer.Write(Environment.NewLine);
            writer.Write("InstanceGroupManager Name", "Zone", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            InstanceGroupManagersResource.ListRequest instanceGroupManagersRequest = computeService.InstanceGroupManagers.List(project, ZoneName);
            ComputeServiceData.InstanceGroupManagerList instanceGroupManagersResponse = instanceGroupManagersRequest.Execute();
            do
            {
                if (instanceGroupManagersResponse.Items == null)
                {
                    continue;
                }

                foreach (ComputeServiceData.InstanceGroupManager instanceGroupManager in instanceGroupManagersResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", instanceGroupManager.Name, ZoneName, subscription.SubscriptionFriendlyName, instanceGroupManager.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (instanceGroupManagersResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the InstanceGroups.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapInstanceGroups(Subscription subscription, StreamWriter writer, string ZoneName)
        {
            writer.Write("InstanceGroups");
            writer.Write(Environment.NewLine);
            writer.Write("InstanceGroup Name", "Zone", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            InstanceGroupsResource.ListRequest instanceGroupsRequest = computeService.InstanceGroups.List(project, ZoneName);
            ComputeServiceData.InstanceGroupList instanceGroupsResponse = instanceGroupsRequest.Execute();
            do
            {
                if (instanceGroupsResponse.Items == null)
                {
                    continue;
                }

                foreach (ComputeServiceData.InstanceGroup instanceGroup in instanceGroupsResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", instanceGroup.Name, ZoneName, subscription.SubscriptionFriendlyName, instanceGroup.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (instanceGroupsResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the Instances.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapInstances(Subscription subscription, StreamWriter writer, string ZoneName)
        {
            writer.Write("Instances");
            writer.Write(Environment.NewLine);
            writer.Write("Instance Name", "Zone", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            InstancesResource.ListRequest instanceRequest = computeService.Instances.List(project, ZoneName);
            ComputeServiceData.InstanceList instanceResponse;
            do
            {
                instanceResponse = instanceRequest.Execute();
                if (instanceResponse.Items == null)
                {
                    continue;
                }

                foreach (ComputeServiceData.Instance instance in instanceResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", instance.Name, ZoneName, subscription.SubscriptionFriendlyName, instance.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (instanceResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the InstanceTemplates.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapInstanceTemplates(Subscription subscription, StreamWriter writer)
        {
            writer.Write("InstanceTemplates");
            writer.Write(Environment.NewLine);
            writer.Write("InstanceTemplate Name", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            InstanceTemplatesResource.ListRequest instanceTemplatesRequest = computeService.InstanceTemplates.List(project);
            ComputeServiceData.InstanceTemplateList instanceTemplatesResponse = instanceTemplatesRequest.Execute();

            do
            {
                if (instanceTemplatesResponse.Items == null)
                {
                    continue;
                }

                foreach (var instanceTemplate in instanceTemplatesResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", instanceTemplate.Name, subscription.SubscriptionFriendlyName, instanceTemplate.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (instanceTemplatesResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the Addresses.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapAddresses(Subscription subscription, StreamWriter writer, string RegionName)
        {
            writer.Write("Addresses");
            writer.Write(Environment.NewLine);
            writer.Write("Addresse Name", "Region", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            AddressesResource.ListRequest addressRequest = computeService.Addresses.List(project, RegionName);
            ComputeServiceData.AddressList addressResponse = addressRequest.Execute();

            do
            {
                if (addressResponse.Items == null)
                {
                    continue;
                }

                foreach (var address in addressResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", address.Name, RegionName, subscription.SubscriptionFriendlyName, address.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (addressResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the Subnetworks.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapSubnetworks(Subscription subscription, StreamWriter writer, string RegionName)
        {
            writer.Write("Subnetworks");
            writer.Write(Environment.NewLine);
            writer.Write("Subnetwork Name", "Region", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            SubnetworksResource.ListRequest subnetRequest = computeService.Subnetworks.List(project, RegionName);
            ComputeServiceData.SubnetworkList subnetResponse = subnetRequest.Execute();
            do
            {
                if (subnetResponse.Items == null)
                {
                    continue;
                }

                foreach (var subnet in subnetResponse.Items)
                {
                    if (!subnet.Name.Equals("default"))
                    {
                        writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", subnet.Name, RegionName, subscription.SubscriptionFriendlyName, subnet.CreationTimestamp, ""));
                        writer.Write(Environment.NewLine);
                    }
                }
            } while (subnetResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the VPCNetwork.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapVPCNetwork(Subscription subscription, StreamWriter writer)
        {
            writer.Write("VPCNetworks");
            writer.Write(Environment.NewLine);
            writer.Write("VPCNetwork Name", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            NetworksResource.ListRequest vpcNetworkRequest = computeService.Networks.List(project);
            ComputeServiceData.NetworkList vpcNetworkResponse = vpcNetworkRequest.Execute();
            do
            {
                if (vpcNetworkResponse.Items == null)
                {
                    continue;
                }

                foreach (var vpcNetwork in vpcNetworkResponse.Items)
                {
                    if (!vpcNetwork.Name.Equals("default"))
                    {
                        writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", vpcNetwork.Name, subscription.SubscriptionFriendlyName, vpcNetwork.CreationTimestamp, ""));
                        writer.Write(Environment.NewLine);
                    }
                }
            } while (vpcNetworkResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the VPCNetworkPeerings.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapVPCNetworkPeerings(Subscription subscription, StreamWriter writer)
        {
            writer.Write("VPCNetworks Peerings");
            writer.Write(Environment.NewLine);
            writer.Write("VPCNetwork Peerings Name", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            NetworksResource.ListRequest vpcNetworkRequest = computeService.Networks.List(project);
            ComputeServiceData.NetworkList vpcNetworkResponse = vpcNetworkRequest.Execute();
            do
            {
                if (vpcNetworkResponse.Items == null)
                {
                    continue;
                }

                foreach (var vpcNetwork in vpcNetworkResponse.Items)
                {
                    if (!vpcNetwork.Name.Equals("default"))
                    {

                        if (vpcNetwork.Peerings != null && vpcNetwork.Peerings.Count() > 0)
                        {
                            foreach (var peering in vpcNetwork.Peerings)
                            {
                                writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", peering.Name, subscription.SubscriptionFriendlyName, "", peering.State));
                                writer.Write(Environment.NewLine);
                            }
                        }
                    }
                }
            } while (vpcNetworkResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the HealthChecks.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapHealthChecks(Subscription subscription, StreamWriter writer)
        {
            writer.Write("HealthChecks");
            writer.Write(Environment.NewLine);
            writer.Write("HealthCheck Name", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            HealthChecksResource.ListRequest healthCheckRequest = computeService.HealthChecks.List(project);
            ComputeServiceData.HealthCheckList healthCheckResponse = healthCheckRequest.Execute();
            do
            {
                if (healthCheckResponse.Items == null)
                {
                    continue;
                }

                foreach (var healthCheck in healthCheckResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", healthCheck.Name, subscription.SubscriptionFriendlyName, healthCheck.CreationTimestamp, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (healthCheckResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the Deployments.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapDeployments(Subscription subscription, StreamWriter writer)
        {
            writer.Write("Deployments");
            writer.Write(Environment.NewLine);
            writer.Write("Deployment Name", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;
            DeploymentManagerService deploymentManagerService = new DeploymentManagerService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-ComputeSample/0.1",
            });

            DeploymentsResource.ListRequest deploymentRequest = deploymentManagerService.Deployments.List(project);
            DeploymentManagerData.DeploymentsListResponse deploymentResponse = deploymentRequest.Execute();
            do
            {
                if (deploymentResponse.Deployments == null)
                {
                    continue;
                }

                foreach (var deployment in deploymentResponse.Deployments)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", deployment.Name, subscription.SubscriptionFriendlyName, deployment.InsertTime, ""));
                    writer.Write(Environment.NewLine);
                }
            } while (deploymentResponse.NextPageToken != null);

            return writer;
        }

        /// <summary>
        /// Snaps the StorageBuckets.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapStorageBuckets(Subscription subscription, StreamWriter writer)
        {
            writer.Write("StorageBuckets");
            writer.Write(Environment.NewLine);
            writer.Write("StorageBucket Name", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;

            var credential = GetCredential(subscription.AppId, subscription.AppSecret, scope);
            var storageClient = StorageClient.Create(credential);

            var buckets = storageClient.ListBuckets(project).ToList();
            foreach (var bucket in buckets)
            {
                writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", bucket.Name, subscription.SubscriptionFriendlyName, bucket.TimeCreated, ""));
                writer.Write(Environment.NewLine);
            }

            return writer;
        }

        /// <summary>
        /// Snaps the CloudSQLDBInstances.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public static StreamWriter SnapCloudSQLDBInstances(Subscription subscription, StreamWriter writer)
        {
            writer.Write("CloudSQLDBInstances");
            writer.Write(Environment.NewLine);
            writer.Write("CloudSQLDBInstance Name", "Region", "Project", "Created Time", "Status");
            writer.Write(Environment.NewLine);

            string scope = ConfigurationManager.AppSettings["Scope"];
            var project = subscription.SubscriptionFriendlyName;

            SQLAdmin.SQLAdminService sqlAdminService = new SQLAdmin.SQLAdminService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(ConfigurationManager.AppSettings["AdminUserName"], ConfigurationManager.AppSettings["ServiceAccountKey"], scope),
                ApplicationName = "Google-SQLAdminSample/0.1",
            });

            SQLAdmin.InstancesResource.ListRequest cloudSQLInstanceRequest = sqlAdminService.Instances.List(project);
            SQLAdminData.InstancesListResponse cloudSQLInstanceResponse = cloudSQLInstanceRequest.Execute();

            do
            {
                if (cloudSQLInstanceResponse.Items == null)
                {
                    continue;
                }

                foreach (SQLAdminData.DatabaseInstance databaseInstance in cloudSQLInstanceResponse.Items)
                {
                    writer.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", databaseInstance.Name, databaseInstance.Region, subscription.SubscriptionFriendlyName, "", databaseInstance.State));
                    writer.Write(Environment.NewLine);
                }
            } while (cloudSQLInstanceResponse.NextPageToken != null);

            return writer;
        }

        public static GoogleCredential GetCredential(string adminUserName, string serviceAccountKey, string scope)
        {
            GoogleCredential credential = GoogleCredential.FromJson(serviceAccountKey);
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(scope).CreateWithUser(adminUserName);
            }

            return credential;
        }
    }
}
