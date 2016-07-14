﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3634
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFServiceGeneratedByConfig
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StudentInfo", Namespace="http://schemas.datacontract.org/2004/07/WCFServiceGeneratedByConfig")]
    public partial class StudentInfo : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string FirstNameField;
        
        private string LastNameField;
        
        private int StudentIDField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FirstName
        {
            get
            {
                return this.FirstNameField;
            }
            set
            {
                this.FirstNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastName
        {
            get
            {
                return this.LastNameField;
            }
            set
            {
                this.LastNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StudentID
        {
            get
            {
                return this.StudentIDField;
            }
            set
            {
                this.StudentIDField = value;
            }
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IStudentService")]
public interface IStudentService
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStudentService/GetStudentFullName", ReplyAction="http://tempuri.org/IStudentService/GetStudentFullNameResponse")]
    string GetStudentFullName(int studentId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStudentService/GetStudentInfo", ReplyAction="http://tempuri.org/IStudentService/GetStudentInfoResponse")]
    WCFServiceGeneratedByConfig.StudentInfo[] GetStudentInfo(int studentId);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface IStudentServiceChannel : IStudentService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class StudentServiceClient : System.ServiceModel.ClientBase<IStudentService>, IStudentService
{
    
    public StudentServiceClient()
    {
    }
    
    public StudentServiceClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public StudentServiceClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public StudentServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public StudentServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public string GetStudentFullName(int studentId)
    {
        return base.Channel.GetStudentFullName(studentId);
    }
    
    public WCFServiceGeneratedByConfig.StudentInfo[] GetStudentInfo(int studentId)
    {
        return base.Channel.GetStudentInfo(studentId);
    }
}