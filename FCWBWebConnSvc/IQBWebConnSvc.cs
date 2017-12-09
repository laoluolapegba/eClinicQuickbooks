using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;



namespace developer.intuit.com
{
    using System.Runtime.Serialization;


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ArrayOfString", Namespace = "http://developer.intuit.com/", ItemName = "string")]
    
    public class ArrayOfString : System.Collections.Generic.List<string>
    {
    }
}



//[System.Web.Services.WebServiceBindingAttribute(Name = "QuickBookConnectorService", Namespace = "http://developer.intuit.com/")]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
//[System.ServiceModel.ServiceContractAttribute(Namespace = "http://developer.intuit.com/", SessionMode = SessionMode.Allowed)]
[System.ServiceModel.ServiceContractAttribute(Namespace = "http://developer.intuit.com/", ConfigurationName = "IQBWebConnAPISoap", SessionMode = SessionMode.Allowed)]
public interface IQBWebConnSvcSoap
{

    // CODEGEN: Generating message contract since element name strVersion from namespace http://developer.intuit.com/ is not marked nillable
    [System.ServiceModel.OperationContractAttribute(Action = "http://developer.intuit.com/serverVersion")]    
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]

    serverVersionResponse serverVersion(serverVersionRequest request);

    // CODEGEN: Generating message contract since element name strVersion from namespace http://developer.intuit.com/ is not marked nillable
    [System.ServiceModel.OperationContractAttribute(Action = "http://developer.intuit.com/clientVersion")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
    clientVersionResponse clientVersion(clientVersionRequest request);

    // CODEGEN: Generating message contract since element name strUserName from namespace http://developer.intuit.com/ is not marked nillable
    [System.ServiceModel.OperationContractAttribute(Action = "http://developer.intuit.com/authenticate")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
    authenticateResponse authenticate(authenticateRequest request);

    // CODEGEN: Generating message contract since element name ticket from namespace http://developer.intuit.com/ is not marked nillable
    [System.ServiceModel.OperationContractAttribute(Action = "http://developer.intuit.com/sendRequestXML")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
    sendRequestXMLResponse sendRequestXML(sendRequestXMLRequest request);

    // CODEGEN: Generating message contract since element name ticket from namespace http://developer.intuit.com/ is not marked nillable
    [System.ServiceModel.OperationContractAttribute(Action = "http://developer.intuit.com/receiveResponseXML")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
    receiveResponseXMLResponse receiveResponseXML(receiveResponseXMLRequest request);

    // CODEGEN: Generating message contract since element name ticket from namespace http://developer.intuit.com/ is not marked nillable
    [System.ServiceModel.OperationContractAttribute(Action = "http://developer.intuit.com/connectionError")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
    connectionErrorResponse connectionError(connectionErrorRequest request);

    // CODEGEN: Generating message contract since element name ticket from namespace http://developer.intuit.com/ is not marked nillable
    [System.ServiceModel.OperationContractAttribute(Action = "http://developer.intuit.com/getLastError")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
    getLastErrorResponse getLastError(getLastErrorRequest request);

    // CODEGEN: Generating message contract since element name ticket from namespace http://developer.intuit.com/ is not marked nillable
    [System.ServiceModel.OperationContractAttribute(Action = "http://developer.intuit.com/closeConnection")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
    closeConnectionResponse closeConnection(closeConnectionRequest request);
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class serverVersionRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "serverVersion", Namespace = "http://developer.intuit.com/", Order = 0)]
    public serverVersionRequestBody Body;

    public serverVersionRequest()
    {
    }

    public serverVersionRequest(serverVersionRequestBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class serverVersionRequestBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string strVersion;

    public serverVersionRequestBody()
    {
    }

    public serverVersionRequestBody(string strVersion)
    {
        this.strVersion = strVersion;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class serverVersionResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "serverVersionResponse", Namespace = "http://developer.intuit.com/", Order = 0)]
    public serverVersionResponseBody Body;

    public serverVersionResponse()
    {
    }

    public serverVersionResponse(serverVersionResponseBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class serverVersionResponseBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string serverVersionResult;

    public serverVersionResponseBody()
    {
    }

    public serverVersionResponseBody(string serverVersionResult)
    {
        this.serverVersionResult = serverVersionResult;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class clientVersionRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "clientVersion", Namespace = "http://developer.intuit.com/", Order = 0)]
    public clientVersionRequestBody Body;

    public clientVersionRequest()
    {
    }

    public clientVersionRequest(clientVersionRequestBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class clientVersionRequestBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string strVersion;

    public clientVersionRequestBody()
    {
    }

    public clientVersionRequestBody(string strVersion)
    {
        this.strVersion = strVersion;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class clientVersionResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "clientVersionResponse", Namespace = "http://developer.intuit.com/", Order = 0)]
    public clientVersionResponseBody Body;

    public clientVersionResponse()
    {
    }

    public clientVersionResponse(clientVersionResponseBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class clientVersionResponseBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string clientVersionResult;

    public clientVersionResponseBody()
    {
    }

    public clientVersionResponseBody(string clientVersionResult)
    {
        this.clientVersionResult = clientVersionResult;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class authenticateRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "authenticate", Namespace = "http://developer.intuit.com/", Order = 0)]
    public authenticateRequestBody Body;

    public authenticateRequest()
    {
    }

    public authenticateRequest(authenticateRequestBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class authenticateRequestBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string strUserName;

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
    public string strPassword;

    public authenticateRequestBody()
    {
    }

    public authenticateRequestBody(string strUserName, string strPassword)
    {
        this.strUserName = strUserName;
        this.strPassword = strPassword;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class authenticateResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "authenticateResponse", Namespace = "http://developer.intuit.com/", Order = 0)]
    public authenticateResponseBody Body;

    public authenticateResponse()
    {
    }

    public authenticateResponse(authenticateResponseBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class authenticateResponseBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public developer.intuit.com.ArrayOfString authenticateResult;

    public authenticateResponseBody()
    {
    }

    public authenticateResponseBody(developer.intuit.com.ArrayOfString authenticateResult)
    {
        this.authenticateResult = authenticateResult;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class sendRequestXMLRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "sendRequestXML", Namespace = "http://developer.intuit.com/", Order = 0)]
    public sendRequestXMLRequestBody Body;

    public sendRequestXMLRequest()
    {
    }

    public sendRequestXMLRequest(sendRequestXMLRequestBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class sendRequestXMLRequestBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string ticket;

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
    public string strHCPResponse;

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
    public string strCompanyFileName;

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
    public string qbXMLCountry;

    [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
    public int qbXMLMajorVers;

    [System.Runtime.Serialization.DataMemberAttribute(Order = 5)]
    public int qbXMLMinorVers;

    public sendRequestXMLRequestBody()
    {
    }

    public sendRequestXMLRequestBody(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
    {
        this.ticket = ticket;
        this.strHCPResponse = strHCPResponse;
        this.strCompanyFileName = strCompanyFileName;
        this.qbXMLCountry = qbXMLCountry;
        this.qbXMLMajorVers = qbXMLMajorVers;
        this.qbXMLMinorVers = qbXMLMinorVers;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class sendRequestXMLResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "sendRequestXMLResponse", Namespace = "http://developer.intuit.com/", Order = 0)]
    public sendRequestXMLResponseBody Body;

    public sendRequestXMLResponse()
    {
    }

    public sendRequestXMLResponse(sendRequestXMLResponseBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class sendRequestXMLResponseBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string sendRequestXMLResult;

    public sendRequestXMLResponseBody()
    {
    }

    public sendRequestXMLResponseBody(string sendRequestXMLResult)
    {
        this.sendRequestXMLResult = sendRequestXMLResult;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class receiveResponseXMLRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "receiveResponseXML", Namespace = "http://developer.intuit.com/", Order = 0)]
    public receiveResponseXMLRequestBody Body;

    public receiveResponseXMLRequest()
    {
    }

    public receiveResponseXMLRequest(receiveResponseXMLRequestBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class receiveResponseXMLRequestBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string ticket;

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
    public string response;

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
    public string hresult;

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
    public string message;

    public receiveResponseXMLRequestBody()
    {
    }

    public receiveResponseXMLRequestBody(string ticket, string response, string hresult, string message)
    {
        this.ticket = ticket;
        this.response = response;
        this.hresult = hresult;
        this.message = message;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class receiveResponseXMLResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "receiveResponseXMLResponse", Namespace = "http://developer.intuit.com/", Order = 0)]
    public receiveResponseXMLResponseBody Body;

    public receiveResponseXMLResponse()
    {
    }

    public receiveResponseXMLResponse(receiveResponseXMLResponseBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class receiveResponseXMLResponseBody
{

    [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
    public int receiveResponseXMLResult;

    public receiveResponseXMLResponseBody()
    {
    }

    public receiveResponseXMLResponseBody(int receiveResponseXMLResult)
    {
        this.receiveResponseXMLResult = receiveResponseXMLResult;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class connectionErrorRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "connectionError", Namespace = "http://developer.intuit.com/", Order = 0)]
    public connectionErrorRequestBody Body;

    public connectionErrorRequest()
    {
    }

    public connectionErrorRequest(connectionErrorRequestBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class connectionErrorRequestBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string ticket;

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
    public string hresult;

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
    public string message;

    public connectionErrorRequestBody()
    {
    }

    public connectionErrorRequestBody(string ticket, string hresult, string message)
    {
        this.ticket = ticket;
        this.hresult = hresult;
        this.message = message;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class connectionErrorResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "connectionErrorResponse", Namespace = "http://developer.intuit.com/", Order = 0)]
    public connectionErrorResponseBody Body;

    public connectionErrorResponse()
    {
    }

    public connectionErrorResponse(connectionErrorResponseBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class connectionErrorResponseBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string connectionErrorResult;

    public connectionErrorResponseBody()
    {
    }

    public connectionErrorResponseBody(string connectionErrorResult)
    {
        this.connectionErrorResult = connectionErrorResult;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class getLastErrorRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "getLastError", Namespace = "http://developer.intuit.com/", Order = 0)]
    public getLastErrorRequestBody Body;

    public getLastErrorRequest()
    {
    }

    public getLastErrorRequest(getLastErrorRequestBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class getLastErrorRequestBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string ticket;

    public getLastErrorRequestBody()
    {
    }

    public getLastErrorRequestBody(string ticket)
    {
        this.ticket = ticket;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class getLastErrorResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "getLastErrorResponse", Namespace = "http://developer.intuit.com/", Order = 0)]
    public getLastErrorResponseBody Body;

    public getLastErrorResponse()
    {
    }

    public getLastErrorResponse(getLastErrorResponseBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class getLastErrorResponseBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string getLastErrorResult;

    public getLastErrorResponseBody()
    {
    }

    public getLastErrorResponseBody(string getLastErrorResult)
    {
        this.getLastErrorResult = getLastErrorResult;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class closeConnectionRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "closeConnection", Namespace = "http://developer.intuit.com/", Order = 0)]
    public closeConnectionRequestBody Body;

    public closeConnectionRequest()
    {
    }

    public closeConnectionRequest(closeConnectionRequestBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class closeConnectionRequestBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string ticket;

    public closeConnectionRequestBody()
    {
    }

    public closeConnectionRequestBody(string ticket)
    {
        this.ticket = ticket;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class closeConnectionResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Name = "closeConnectionResponse", Namespace = "http://developer.intuit.com/", Order = 0)]
    public closeConnectionResponseBody Body;

    public closeConnectionResponse()
    {
    }

    public closeConnectionResponse(closeConnectionResponseBody Body)
    {
        this.Body = Body;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://developer.intuit.com/")]
public partial class closeConnectionResponseBody
{

    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
    public string closeConnectionResult;

    public closeConnectionResponseBody()
    {
    }

    public closeConnectionResponseBody(string closeConnectionResult)
    {
        this.closeConnectionResult = closeConnectionResult;
    }
}

