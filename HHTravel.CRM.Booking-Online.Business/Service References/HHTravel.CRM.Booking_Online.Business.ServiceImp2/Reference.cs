﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HHTravel.CRM.Booking_Online.Business.HHTravel.CRM.Booking_Online.Business.ServiceImp2 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="HHTravel.CRM.Booking_Online.Business.ServiceImp2.IProductService")]
    public interface IProductService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductService/FindByDepartureDate", ReplyAction="http://tempuri.org/IProductService/FindByDepartureDateResponse")]
        HHTravel.CRM.Booking_Online.Model.Product[] FindByDepartureDate(System.DateTime beginDate, System.DateTime endDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductService/FindByDestination", ReplyAction="http://tempuri.org/IProductService/FindByDestinationResponse")]
        HHTravel.CRM.Booking_Online.Model.Product[] FindByDestination(int groupId, System.Nullable<int> regionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductService/FindByTravelType", ReplyAction="http://tempuri.org/IProductService/FindByTravelTypeResponse")]
        HHTravel.CRM.Booking_Online.Model.Product[] FindByTravelType(int travelTypeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductService/GetAllDepartureMonths", ReplyAction="http://tempuri.org/IProductService/GetAllDepartureMonthsResponse")]
        HHTravel.CRM.Booking_Online.Model.DepartureMonth[] GetAllDepartureMonths();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductService/GetAllDeparturePlaces", ReplyAction="http://tempuri.org/IProductService/GetAllDeparturePlacesResponse")]
        HHTravel.CRM.Booking_Online.Model.DeparturePlace[] GetAllDeparturePlaces();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductService/GetAllDestinationGroups", ReplyAction="http://tempuri.org/IProductService/GetAllDestinationGroupsResponse")]
        HHTravel.CRM.Booking_Online.Model.DestinationGroup[] GetAllDestinationGroups();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductService/GetAllInterests", ReplyAction="http://tempuri.org/IProductService/GetAllInterestsResponse")]
        HHTravel.CRM.Booking_Online.Model.Interest[] GetAllInterests();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductService/GetAllProducts", ReplyAction="http://tempuri.org/IProductService/GetAllProductsResponse")]
        HHTravel.CRM.Booking_Online.Model.Product[] GetAllProducts();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductService/GetAllTravelTypes", ReplyAction="http://tempuri.org/IProductService/GetAllTravelTypesResponse")]
        HHTravel.CRM.Booking_Online.Model.TravelType[] GetAllTravelTypes();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductService/Search", ReplyAction="http://tempuri.org/IProductService/SearchResponse")]
        HHTravel.CRM.Booking_Online.Model.Product[] Search(System.Nullable<int> departurePlaceId, System.Nullable<int> destinationGroupId, System.Nullable<int> destinationRegionId, System.Nullable<int> travelTypeId, System.Nullable<System.DateTime> beginDate, System.Nullable<System.DateTime> endDate, System.Nullable<int> interestid);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IProductServiceChannel : HHTravel.CRM.Booking_Online.Business.HHTravel.CRM.Booking_Online.Business.ServiceImp2.IProductService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ProductServiceClient : System.ServiceModel.ClientBase<HHTravel.CRM.Booking_Online.Business.HHTravel.CRM.Booking_Online.Business.ServiceImp2.IProductService>, HHTravel.CRM.Booking_Online.Business.HHTravel.CRM.Booking_Online.Business.ServiceImp2.IProductService {
        
        public ProductServiceClient() {
        }
        
        public ProductServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ProductServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProductServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProductServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public HHTravel.CRM.Booking_Online.Model.Product[] FindByDepartureDate(System.DateTime beginDate, System.DateTime endDate) {
            return base.Channel.FindByDepartureDate(beginDate, endDate);
        }
        
        public HHTravel.CRM.Booking_Online.Model.Product[] FindByDestination(int groupId, System.Nullable<int> regionId) {
            return base.Channel.FindByDestination(groupId, regionId);
        }
        
        public HHTravel.CRM.Booking_Online.Model.Product[] FindByTravelType(int travelTypeId) {
            return base.Channel.FindByTravelType(travelTypeId);
        }
        
        public HHTravel.CRM.Booking_Online.Model.DepartureMonth[] GetAllDepartureMonths() {
            return base.Channel.GetAllDepartureMonths();
        }
        
        public HHTravel.CRM.Booking_Online.Model.DeparturePlace[] GetAllDeparturePlaces() {
            return base.Channel.GetAllDeparturePlaces();
        }
        
        public HHTravel.CRM.Booking_Online.Model.DestinationGroup[] GetAllDestinationGroups() {
            return base.Channel.GetAllDestinationGroups();
        }
        
        public HHTravel.CRM.Booking_Online.Model.Interest[] GetAllInterests() {
            return base.Channel.GetAllInterests();
        }
        
        public HHTravel.CRM.Booking_Online.Model.Product[] GetAllProducts() {
            return base.Channel.GetAllProducts();
        }
        
        public HHTravel.CRM.Booking_Online.Model.TravelType[] GetAllTravelTypes() {
            return base.Channel.GetAllTravelTypes();
        }
        
        public HHTravel.CRM.Booking_Online.Model.Product[] Search(System.Nullable<int> departurePlaceId, System.Nullable<int> destinationGroupId, System.Nullable<int> destinationRegionId, System.Nullable<int> travelTypeId, System.Nullable<System.DateTime> beginDate, System.Nullable<System.DateTime> endDate, System.Nullable<int> interestid) {
            return base.Channel.Search(departurePlaceId, destinationGroupId, destinationRegionId, travelTypeId, beginDate, endDate, interestid);
        }
    }
}