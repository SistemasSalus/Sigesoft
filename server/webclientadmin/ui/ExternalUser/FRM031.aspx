<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FRM031.aspx.cs" Inherits="Sigesoft.Server.WebClientAdmin.UI.ExternalUser.FRM031" %>
<%@ Register assembly="FineUI" namespace="FineUI" tagprefix="x" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Administración de Servicios</title>
    <link href="../CSS/main.css" rel="stylesheet" />
    <style>
        .StylelblContador {
                color: #0026ff;
                font-weight: bold;
                font-size: 11px;
                text-decoration: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
            <x:Pagemanager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
         <x:Panel ID="Panel1" runat="server"  ShowBorder="True" ShowHeader="True" Title="Administrador de Servicios" EnableBackgroundColor="true" Layout="VBox" 
          BoxConfigAlign="Stretch" BoxConfigPosition="Start" BoxConfigChildMargin="3 7 12 5" >
        <Items>
            <x:GroupPanel runat="server" Title="Búsqueda / Filtro" ID="GroupPanel1" AutoWidth="true" BoxFlex="1" Height="132" >                
                <Items>
                    <x:Form ID="Form2" runat="server" EnableBackgroundColor="true" ShowBorder="False" ShowHeader="False" LabelWidth="90px" LabelAlign="Left">
                            <Rows>
                                <x:FormRow ID="FormRow1" ColumnWidths="460px 460px 100px" runat="server">
                                    <Items> 
                                        <x:Form ID="Form3"   runat="server" EnableBackgroundColor="true" ShowBorder="False" ShowHeader="False" LabelWidth="90px" LabelAlign="Left">
                                            <Rows>
                                                <x:FormRow ID="FormRow2" ColumnWidths="230px 230px" runat="server" >
                                                    <Items>
                                                        <x:DatePicker ID="dpFechaInicio" Label="Atenciones del" Width="120px" runat="server" DateFormatString="dd/MM/yyyy" />
                                                        <x:DatePicker ID="dpFechaFin" Label="Al"  runat="server" Width="120px" DateFormatString="dd/MM/yyyy" />                                       
                                                    </Items>
                                                </x:FormRow>
                                            </Rows>
                                        </x:Form>
                                        <x:Form ID="Form6"  runat="server" EnableBackgroundColor="true" ShowBorder="False" ShowHeader="False" LabelWidth="70px" LabelAlign="Left">
                                            <Rows>
                                                <x:FormRow ColumnWidths="215px 245px"  ID="FormRow5" runat="server" >
                                                    <Items>
                                                         <x:DropDownList ID="ddlTipoESO" Label="Tipo ESO" runat="server" />
                                                        <x:DropDownList ID="ddlAptitud" Label="Aptitud" runat="server" />
                                                    </Items>
                                                </x:FormRow>
                                            </Rows>
                                        </x:Form>
                                         <x:Button ID="btnFilter" Text="Filtrar" Icon="Find" IconAlign="Left" runat="server" AjaxLoadingType="Mask" CssClass="inline" OnClick="btnFilter_Click" ValidateForms="Form2" ></x:Button>                         
                                    </Items>
                                </x:FormRow>
                                <x:FormRow ID="FormRow3" ColumnWidths="460px 460px"  runat="server">
                                    <Items>
                                        <x:Form ID="Form4"  runat="server" EnableBackgroundColor="true" ShowBorder="False" ShowHeader="False" LabelWidth="90px" LabelAlign="Left">
                                            <Rows>
                                                <x:FormRow ColumnWidths="460px" ID="FormRow4" runat="server" >
                                                    <Items>
                                                        <x:TextBox ID="txtTrabajador" Label="Paciente" runat="server"/>  
                                                    </Items>
                                                </x:FormRow>
                                            </Rows>
                                        </x:Form> 
                                        <x:Form ID="Form7"  runat="server" EnableBackgroundColor="true" ShowBorder="False" ShowHeader="False" LabelWidth="70px" LabelAlign="Left">
                                            <Rows>
                                                <x:FormRow ColumnWidths="460px" ID="FormRow7" runat="server" >
                                                    <Items>
                                                  <%--      <x:TextBox ID="txtEmpresa" Label="Empresa" Enabled="false" runat="server"/>  --%>
                                                        <x:DropDownList ID="ddlEmpresa" Label="Empresa" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged" />
                                                    </Items>
                                                </x:FormRow>
                                            </Rows>
                                        </x:Form> 
                                       
                                    </Items>
                                </x:FormRow>
                                <x:FormRow ColumnWidths="460px 460px" ID="FormRow6"  runat="server">
                                    <Items>
                                        <x:Form ID="Form5"  runat="server" EnableBackgroundColor="true" ShowBorder="False" ShowHeader="False" LabelWidth="90px" LabelAlign="Left">
                                            <Rows>
                                                <x:FormRow ColumnWidths="230px 230px" ID="FormRow8" runat="server" >
                                                    <Items>
                                                       <x:TextBox ID="txtHCL" Label="Cod.HC" runat="server"/>
                                                        <x:Label ID="Label1" runat="server" Text=""></x:Label>
                                                    </Items>
                                                </x:FormRow>
                                            </Rows>
                                        </x:Form> 

                                          <x:Form ID="Form8"  runat="server" EnableBackgroundColor="true" ShowBorder="False" ShowHeader="False" LabelWidth="70px" LabelAlign="Left">
                                            <Rows>
                                                <x:FormRow ColumnWidths="460px" ID="FormRow9" runat="server" >
                                                    <Items>                                                        
                                                         <x:DropDownList ID="ddlProtocolo"  Label="Protocolo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProtocolo_SelectedIndexChanged" />  
                                                    </Items>
                                                </x:FormRow>
                                            </Rows>
                                        </x:Form> 
                                        
                                    </Items>
                                </x:FormRow>
                                <x:FormRow  ColumnWidths="840px 100px" ID="FormRow10"  runat="server">
                                    <Items>
                                            <x:Label runat="server" ID="lblContador" Text="Se encontraron 0 registros" Width="800px" CssClass="StylelblContador"></x:Label>                                         
                                    </Items> 
                                </x:FormRow>
                            </Rows>
                    </x:Form>
                </Items>
            </x:GroupPanel>
            <x:Grid ID="grdData" ShowBorder="true" ShowHeader="false" runat="server" 
            PageSize="30" EnableRowNumber="True" AllowPaging="true" OnPageIndexChange="grdData_PageIndexChange"
            IsDatabasePaging="true" EnableRowNumberPaging="true" AutoHeight="true" RowNumberWidth="40" AjaxLoadingType="Default"
            EnableMouseOverColor="true" ShowGridHeader="true" OnRowCommand="grdData_RowCommand" DataKeyNames="v_ServiceId,v_IdTrabajador,EmpresaCliente,v_Trabajador" 
            EnableTextSelection="true" EnableAlternateRowColor="true" EnableCheckBoxSelect="true" BoxFlex="2" BoxMargin="5" 
            OnRowClick="grdData_RowClick" EnableRowClick="true">
                <Toolbars>
                    <x:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <x:Button ID="btnNewCertificado" Text="Certificado de Aptitud" Icon="PageWhiteText" runat="server" Enabled="false" ></x:Button>
                            <x:Button ID="btnNewFichaOcupacional" Text="Informe Medico Trabajador" Icon="clipboard" runat="server" Enabled="false"></x:Button>
                            <x:Button ID="btnNewExamenes" Text="Historia Clínica Completa" Icon="PageWhiteStack" runat="server" Enabled="false"></x:Button>
                            <x:Button ID="btnPlacaRx" Text="Placa Rayos X" Icon="PageWhiteStack" runat="server" Enabled="false"></x:Button>
                            <x:Button ID="btnFichaCovid19" Text="Ficha Covid-19" Icon="PageWhiteStack" runat="server" Enabled="false"></x:Button>
                        </Items>
                    </x:Toolbar>
                </Toolbars>
                <Columns>
                    <%--<x:boundfield Width="140px" DataField="v_ServiceId" DataFormatString="{0}" HeaderText="Id Atencion" />
                    <x:boundfield Width="270px" DataField="v_Trabajador" DataFormatString="{0}" HeaderText="Trabajador" />
                    <x:boundfield Width="150px" DataField="d_ServiceDate" DataFormatString="{0:d}" HeaderText="Fecha" />
                    <x:boundfield Width="200px" DataField="v_AptitudeStatusName" DataFormatString="{0}" HeaderText="Aptitud" />
                    <x:boundfield Width="400px" DataField="v_Restricction" DataFormatString="{0}" HeaderText="Restricciones" />
                    <x:boundfield Width="250px" DataField="v_ProtocolName" DataFormatString="{0}" HeaderText="Protocolo" />--%>

                    <x:boundfield Width="140px" DataField="v_ServiceId" DataFormatString="{0}" HeaderText="Id Atencion" />
                    <x:boundfield Width="0px" DataField="v_PersonId" DataFormatString="{0}" HeaderText="Id Trab" />
                    <x:boundfield Width="270px" DataField="v_Trabajador" DataFormatString="{0}" HeaderText="Trabajador" />
                    <x:boundfield Width="150px" DataField="d_ServiceDate" DataFormatString="{0:d}" HeaderText="Fecha" />
                    <x:boundfield Width="200px" DataField="EmpresaCliente" DataFormatString="{0}" HeaderText="Empresa" />
                    <x:boundfield Width="0px" DataField="v_ProtocolId " DataFormatString="{0}" HeaderText="Id Prot" />
                    <x:boundfield Width="400px" DataField="Protocolo" DataFormatString="{0}" HeaderText="Protocolo" />
                    <x:boundfield Width="250px" DataField="v_AptitudeStatusName" DataFormatString="{0}" HeaderText="ESTADO" />
                    <x:boundfield Width="250px" DataField="Pendiente" DataFormatString="{0}" HeaderText="Pendiente" />  
                </Columns>
            </x:Grid>
        </Items>
    </x:Panel>
    
    <x:HiddenField ID="hfRefresh" runat="server" />

    <x:Window ID="winEdit1" Title="Certificado(s)" Popup="false" EnableIFrame="true" runat="server" IconUrl="~/images/16/11.png" 
       CloseAction="HidePostBack" EnableConfirmOnClose="true"  IFrameUrl="about:blank" EnableMaximize="true" EnableResize="true"
       Target="Top" OnClose="winEdit1_Close"  IsModal="true"  Height="200px" Width="300px" >
    </x:Window>

    <x:Window ID="winEdit2" Title="Informe para el Trabajador" Popup="false" EnableIFrame="true" runat="server" IconUrl="~/images/16/11.png" 
       CloseAction="HideRefresh" EnableConfirmOnClose="true"  IFrameUrl="about:blank" EnableMaximize="true" EnableResize="true"
       Target="Top" OnClose="winEdit2_Close"  IsModal="true"  Height="200px" Width="300px" >
    </x:Window>

    <x:Window ID="winEdit3" IconUrl="~/images/16/11.png" runat="server" Popup="false"
        IsModal="true" Target="Parent" EnableMaximize="false" EnableResize="false"
        Title="Historia Clinica Completa" EnableConfirmOnClose="true" CloseAction="HidePostBack"
        EnableIFrame="true" IFrameUrl="about:blank" Height="200px" Width="300px"  OnClose="winEdit3_Close">
    </x:Window>

        
    <x:Window ID="winEdit4" IconUrl="~/images/16/11.png" runat="server" Popup="false"
        IsModal="true" Target="Parent" EnableMaximize="false" EnableResize="false"
        Title="Placa Rx" EnableConfirmOnClose="true" CloseAction="HidePostBack"
        EnableIFrame="true" IFrameUrl="about:blank" Height="200px" Width="300px"  OnClose="winEdit4_Close">
    </x:Window>

        
    <x:Window ID="winEdit5" IconUrl="~/images/16/11.png" runat="server" Popup="false"
        IsModal="true" Target="Parent" EnableMaximize="false" EnableResize="false"
        Title="Ficha Covid 19" EnableConfirmOnClose="true" CloseAction="HidePostBack"
        EnableIFrame="true" IFrameUrl="about:blank" Height="200px" Width="300px"  OnClose="winEdit5_Close">
    </x:Window>
  
    </form>
</body>
</html>
