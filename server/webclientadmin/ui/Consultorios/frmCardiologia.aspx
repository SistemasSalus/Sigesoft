<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCardiologia.aspx.cs" Inherits="Sigesoft.Server.WebClientAdmin.UI.Consultorios.frmCardiologia" %>
<%@ Register Assembly="FineUI" Namespace="FineUI" TagPrefix="x" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <title></title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
      <style type="text/css">
        .highlight
        {
            background-color: lightgreen;
        }
        .highlight .x-grid3-col
        {
            background-image: none;
        }
        
        .x-grid3-row-selected .highlight
        {
            background-color: yellow;
        }
        .x-grid3-row-selected .highlight .x-grid3-col
        {
            background-image: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <x:PageManager ID="PageManager1" runat="server"/>
        <x:Panel ID="Panel2" runat="server" Height="5000px" Width="1000px" ShowBorder="True"
            Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start" BoxConfigPadding="5"
            BoxConfigChildMargin="0 0 5 0" ShowHeader="True" Title="Consultorio Cardiología">
            <Items>
                <x:GroupPanel runat="server" Title="Búsqueda / Filtro" ID="GroupPanel1" AutoWidth="true" BoxFlex="1" Height="60">
                    <Items>
                        <x:SimpleForm ID="frmFiltro" ShowBorder="False" EnableBackgroundColor="false" ShowHeader="False" runat="server">
                            <Items>
                                <x:Form ID="Form8" runat="server" EnableBackgroundColor="false" ShowBorder="False" ShowHeader="False" LabelWidth="90px" LabelAlign="Left">
                                    <Rows>
                                        <x:FormRow ID="FormRow19" ColumnWidths="460px" runat="server">
                                            <Items>
                                                <x:Form ID="Form9" runat="server" EnableBackgroundColor="false" ShowBorder="False" ShowHeader="False" LabelWidth="40px" LabelAlign="Left">
                                                    <Rows>
                                                        <x:FormRow ID="FormRow20" ColumnWidths="150px 150px 300px 220px 100px" runat="server">
                                                            <Items>
                                                                <x:DatePicker ID="dpFechaInicio" Label="F.I" Width="90px" runat="server" DateFormatString="dd/MM/yyyy" />
                                                                <x:DatePicker ID="dpFechaFin" Label="F.F" runat="server" Width="90px" DateFormatString="dd/MM/yyyy" />                                                              
                                                                <x:DropDownList ID="ddlConsultorio" runat="server" Label="Consul." Width="240px"></x:DropDownList>
                                                                <x:TextBox runat="server" Label="Trabaj." Text="" Width="240px" ID="txtTrabajador"></x:TextBox>
                                                                <x:Button ID="btnFilter" Text="Filtrar" Icon="Find" IconAlign="Left" runat="server" AjaxLoadingType="Mask" CssClass="inline" OnClick="btnFilter_Click"></x:Button>
                                                            </Items>
                                                        </x:FormRow>
                                                    </Rows>
                                                </x:Form>
                                            </Items>
                                        </x:FormRow>
                                    </Rows>
                                </x:Form>
                            </Items>
                        </x:SimpleForm>
                    </Items>
                </x:GroupPanel>
                <x:GroupPanel runat="server" Title="Resultado de la búsqueda" ID="GroupPanel2" AutoWidth="true" BoxFlex="1" Height="280">
                    <Items>
                          <x:Form ID="Form58" runat="server" EnableBackgroundColor="false" ShowBorder="False" ShowHeader="False" LabelWidth="90px" LabelAlign="Left">
                            <Rows>
                                <x:FormRow ID="FormRow206" ColumnWidths="950px" runat="server" >
                                    <Items>
                                        <x:Grid ID="grdData" ShowBorder="true" ShowHeader="false" runat="server" AutoScroll="true" Height="240px"
                                            EnableRowNumber="True" EnableRowNumberPaging="true" RowNumberWidth="40" AjaxLoadingType="Mask"
                                            EnableMouseOverColor="true" ShowGridHeader="true" DataKeyNames="v_ServiceId,v_IdTrabajador,v_Genero,i_AptitudeStatusId,i_EsoTypeId,v_ExploitedMineral,i_AltitudeWorkId,i_PlaceWorkId,v_Pacient,Dni,d_ServiceDate,v_TipoEso,v_Puesto,d_FechaNacimiento,EmpresaCliente,AreaEmpresa,v_SectorName,ComentarioAptitud"
                                            EnableTextSelection="true" EnableAlternateRowColor="true" BoxFlex="2" BoxMargin="5" EnableCheckBoxSelect="false"
                                            OnRowClick="grdData_RowClick" EnableRowClick="true" OnRowCommand="grdData_RowCommand"  OnRowDataBound="grdData_RowDataBound" >                         
                                            <Columns>
                                                 <x:CheckBoxField Width="30px" RenderAsStaticField="true" DataField="AtSchool" HeaderText="" />
                                                <x:BoundField Width="270px" DataField="v_Pacient" DataFormatString="{0}" HeaderText="Trabajador" />
                                                <x:BoundField Width="80px" DataField="d_ServiceDate" DataFormatString="{0:d}" HeaderText="Fecha" />
                                                <x:BoundField Width="300px" DataField="v_ProtocolName" DataFormatString="{0}" HeaderText="Protocolo" />
                                                <x:BoundField Width="140px" DataField="v_ServiceId" DataFormatString="{0}" HeaderText="Id Atencion" />
                                            </Columns>
                                        </x:Grid>                                       
                                    </Items>
                                </x:FormRow>
                            </Rows>
                        </x:Form>
                    </Items>
                </x:GroupPanel>                   
                <x:TabStrip ID="TabStrip1" Width="1000px" Height="5670px" ShowBorder="true" ActiveTabIndex="0" runat="server" EnableTitleBackgroundColor="False">
                    <Tabs>
                        <x:Tab ID="TabElectrocardiograma" BodyPadding="5px" Title="Electrocardiograma" runat="server">
                            <Toolbars>
                                <x:Toolbar ID="Toolbar4" runat="server">
                                    <Items>
                                        <x:Button ID="btnGrabarElectrocardiograma" Text="Grabar Electrocardiograma" Icon="SystemSave" runat="server" OnClick="btnGrabarElectrocardiograma_Click" AjaxLoadingType="Mask"></x:Button>                                   
                                          <x:FileUpload runat="server" ID="fileDoc" EmptyText="Por favor seleccione un archivo" Width="300" Height="25"
                                        Label="Seleccionar Excel" ButtonIcon="SystemSearch" OnFileSelected="fileDoc_FileSelected" AutoPostBack="true" ButtonText="Subir Adjunto" Readonly="False">
                                        </x:FileUpload>
                                         <x:Button ID="btnDescargar" runat="server" Text="Descargar Adjuntos" Icon="ArrowDown"></x:Button>
                                        <x:Label ID="Label746" runat="server" Text="....." ShowLabel="false"></x:Label>                                       
                                        <x:Button ID="btnReporteCardio" Text="Ver Reporte" Icon="PageWhiteText" runat="server" Enabled="true" ></x:Button>
                                    </Items>
                                </x:Toolbar>
                            </Toolbars>
                            <Items>
                                <x:Panel ID="panel1" Title="ANTECEDENTES" EnableBackgroundColor="true" Height="120px" runat="server"
                                    BodyPadding="5px" ShowBorder="true" ShowHeader="true" Enabled="true">
                                    <Items>
                                        <x:Form ID="Form11" runat="server" EnableBackgroundColor="true" ShowBorder="False" ShowHeader="False" LabelWidth="120px" LabelAlign="Left" >
                                            <Rows>
                                                 <x:FormRow ID="FormRow22" ColumnWidths="150px 50px  150px 50px  150px 50px  150px 50px " runat="server" >
                                                    <Items>

                                                        <x:Label ID="label20" runat="server" Text="CANSANCIO" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkCansancio" runat="server" Text="" ShowLabel="false"></x:CheckBox>  

                                                        <x:Label ID="label30" runat="server" Text="DIABETES" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkDiabetes" runat="server" Text="" ShowLabel="false"></x:CheckBox>  
                                                        
                                                         <x:Label ID="label2" runat="server" Text="DISLIPIDEMIA" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkDisplidemia" runat="server" Text="" ShowLabel="false"></x:CheckBox>
                                                        
                                                         <x:Label ID="label21" runat="server" Text="DOLOR PRECORDIAL" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkDolorPrecordial" runat="server" Text="" ShowLabel="false"></x:CheckBox>

                                                                                                                                             
                                                    </Items>
                                                </x:FormRow>
                                                 <x:FormRow ID="FormRow1" ColumnWidths="150px 50px  150px 50px  150px 50px  150px 50px " runat="server" >
                                                    <Items>

                                                         <x:Label ID="label3" runat="server" Text="INFARTO MA" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkIMA" runat="server" Text="" ShowLabel="false"></x:CheckBox>     

                                                         <x:Label ID="label1" runat="server" Text="MAREOS" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkMareos" runat="server" Text="" ShowLabel="false"></x:CheckBox>  
                                                        
                                                         <x:Label ID="label6" runat="server" Text="OBESIDAD" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkObesidad" runat="server" Text="" ShowLabel="false"></x:CheckBox>  
                                                        
                                                         <x:Label ID="label5" runat="server" Text="PALPITACIONES" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkPalpitaciones" runat="server" Text="" ShowLabel="false"></x:CheckBox>  

 
                                                                                                                                                 
                                                    </Items>
                                                </x:FormRow>
                                                 <x:FormRow ID="FormRow2" ColumnWidths="150px 50px  150px 50px  150px 50px  150px 50px " runat="server" >
                                                    <Items>
                                                         <x:Label ID="label7" runat="server" Text="PERDIDA DE CONCIENCIA" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkPerdConciencia" runat="server" Text="" ShowLabel="false"></x:CheckBox> 

                                                        <x:Label ID="label4" runat="server" Text="HIPERTENSIÓN" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkHipertension" runat="server" Text="" ShowLabel="false"></x:CheckBox> 

                                                        <x:Label ID="label8" runat="server" Text="SOPLOS" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkSoplos" runat="server" Text="" ShowLabel="false"></x:CheckBox>  

                                                         <x:Label ID="label9" runat="server" Text="TABAQUISMO" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkTabaquismo" runat="server" Text="" ShowLabel="false"></x:CheckBox>  
                                                                                                                                    
                                                    </Items>
                                                </x:FormRow>
                                                 <x:FormRow ID="FormRow6" ColumnWidths="150px 50px  150px 450px " runat="server" >
                                                    <Items>
                                                        <x:Label ID="label10" runat="server" Text="VARISES MMII" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkVarisesMMII" runat="server" Text="" ShowLabel="false"></x:CheckBox>  

                                                         <x:Label ID="label11" runat="server" Text="OTROS" ShowLabel="false"></x:Label>
                                                        <x:TextBox ID="txtOtros" runat="server" Text="" ShowLabel="false"></x:TextBox>             
                                                    </Items>
                                                </x:FormRow>
                                            </Rows>
                                        </x:Form>
                                    </Items>
                                </x:Panel>
                                <x:Panel ID="panel3" Title="FRECUENCIA CARDIACA" EnableBackgroundColor="true" Height="40px" runat="server"
                                    BodyPadding="5px" ShowBorder="true" ShowHeader="true" Enabled="true">
                                    <Items>
                                        <x:TextBox ID="txtFrecuenciaCardiaca" runat="server" ShowLabel="false"></x:TextBox>
                                    </Items>
                                </x:Panel>
                                <x:Panel ID="panel4" Title="INFORME DESCRIPTIVO" EnableBackgroundColor="true" Height="60px" runat="server"
                                    BodyPadding="5px" ShowBorder="true" ShowHeader="true" Enabled="true">
                                    <Items>
                                        <x:Form ID="Form2" runat="server" EnableBackgroundColor="true" ShowBorder="False" ShowHeader="False" LabelWidth="120px" LabelAlign="Left" >
                                            <Rows>
                                                 <x:FormRow ID="FormRow3" ColumnWidths="150px 80px  150px 80px  150px 80px  " runat="server" >
                                                    <Items>
                                                        <x:Label ID="label12" runat="server" Text="INTERVALO PR (seg)" ShowLabel="false"></x:Label>
                                                        <x:TextBox ID="txtIntervPR" runat="server" Text="" ShowLabel="false"></x:TextBox>  

                                                         <x:Label ID="label13" runat="server" Text="INTERVALO QRS (seg)" ShowLabel="false"></x:Label>
                                                        <x:TextBox ID="txtInterQRS" runat="server" Text="" ShowLabel="false"></x:TextBox>  

                                                         <x:Label ID="label14" runat="server" Text="INTERVALO QT (seg)" ShowLabel="false"></x:Label>
                                                        <x:TextBox ID="txtInterQT" runat="server" Text="" ShowLabel="false"></x:TextBox>  
                                                                                                                                                                                                 
                                                    </Items>
                                                </x:FormRow>
                                                  <x:FormRow ID="FormRow4" ColumnWidths="150px 80px  150px 80px  150px 160px  " runat="server" >
                                                    <Items>
                                                        <x:Label ID="label15" runat="server" Text="SEGMENTO ST (seg)" ShowLabel="false"></x:Label>
                                                        <x:TextBox ID="txtEjeST" runat="server" Text="" ShowLabel="false"></x:TextBox>  

                                                         <x:Label ID="label16" runat="server" Text="EJE QRS (seg)" ShowLabel="false"></x:Label>
                                                        <x:TextBox ID="txtEjeQRS" runat="server" Text="" ShowLabel="false"></x:TextBox>  

                                                         <x:Label ID="label17" runat="server" Text="RITMO (seg)" ShowLabel="false"></x:Label>
                                                        <x:TextBox ID="txtRitmo" runat="server" Text="" ShowLabel="false"></x:TextBox>  
                                                                                                                                                                                                 
                                                    </Items>
                                                </x:FormRow>
                                            </Rows>
                                        </x:Form>
                                    </Items>
                                </x:Panel>
                                <x:Panel ID="panel5" Title="INTERPRETACION" EnableBackgroundColor="true" Height="40px" runat="server"
                                    BodyPadding="5px" ShowBorder="true" ShowHeader="true" Enabled="true">
                                    <Items>
                                        <x:TextBox ID="txtInterpretacion" runat="server" ShowLabel="false" Width="960"></x:TextBox>
                                    </Items>
                                </x:Panel>
                                <x:Panel ID="panel6" Title="RECOMENDACIONES" EnableBackgroundColor="true" Height="40px" runat="server"
                                    BodyPadding="5px" ShowBorder="true" ShowHeader="true" Enabled="true">
                                    <Items>
                                        <x:TextBox ID="txtRecomendaciones" runat="server" ShowLabel="false" Width="960"></x:TextBox>
                                    </Items>
                                </x:Panel>
                                <x:Panel ID="panel7" Title="APTITUD" EnableBackgroundColor="true" Height="40px" runat="server"
                                    BodyPadding="5px" ShowBorder="true" ShowHeader="true" Enabled="true">
                                    <Items>
                                        <x:Form ID="Form3" runat="server" EnableBackgroundColor="true" ShowBorder="False" ShowHeader="False" LabelWidth="120px" LabelAlign="Left" >
                                            <Rows>
                                                <x:FormRow ID="FormRow5" ColumnWidths="250px 50px  250px 400px  " runat="server" >
                                                    <Items>
                                                        <x:Label ID="label18" runat="server" Text="APTO TRAB FORZOSO" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkAptoTrabForzoso" runat="server" Text="" ShowLabel="false"></x:CheckBox>  

                                                        <x:Label ID="label19" runat="server" Text="APTO TRAB 2500 MSNM" ShowLabel="false"></x:Label>
                                                        <x:CheckBox ID="chkAptoTrab2500" runat="server" Text="" ShowLabel="false" Width="600"></x:CheckBox>                                                                                          
                                                    </Items>
                                                </x:FormRow>
                                            </Rows>
                                        </x:Form>
                                    </Items>
                                </x:Panel>
                                
<%--                                <x:Panel ID="panel7" Title="SUBIR/DESCARGAR ARCHIVO" EnableBackgroundColor="true" Height="40px" runat="server"
                                    BodyPadding="5px" ShowBorder="true" ShowHeader="true" Enabled="true">
                                    <Items>
                                    </Items>
                                </x:Panel>--%>
                          
                            </Items>
                        </x:Tab>
                         <x:Tab ID="TabVacio" BodyPadding="5px" Title=" " runat="server">
                            <Items>

                            </Items>
                        </x:Tab>
                    </Tabs>
                </x:TabStrip>
            </Items>
        </x:Panel>
          <x:HiddenField ID="hfRefresh" runat="server" />
        <x:Window ID="Window2" Title="Descargar" Popup="false" EnableIFrame="true" runat="server" Icon="UserBrown"
            CloseAction="HidePostBack" EnableConfirmOnClose="true" IFrameUrl="about:blank" EnableMaximize="false" EnableResize="false" 
            Target="Top"  IsModal="True" Width="450px" Height="370px" >
        </x:Window>
        <x:Window ID="WindowAddDX" Title="Nuevo Diagnóstico" Popup="false" EnableIFrame="true" runat="server" Icon="UserBrown"
            CloseAction="HidePostBack" EnableConfirmOnClose="true" IFrameUrl="about:blank" EnableMaximize="false" EnableResize="false"
            Target="Top" OnClose="WindowAddDX_Close" IsModal="True" Width="650px" Height="480px">
        </x:Window>

        <x:Window ID="WindowAddDXFrecuente" Title="Diagnóstico Frecuente" Popup="false" EnableIFrame="true" runat="server" Icon="UserBrown"
            CloseAction="HidePostBack" EnableConfirmOnClose="true" IFrameUrl="about:blank" EnableMaximize="false" EnableResize="false"
            Target="Top" OnClose="WindowAddDXFrecuente_Close" IsModal="True" Width="650px" Height="640px">
        </x:Window>

            <x:Window ID="winEdit1" Title="Reporte" Popup="false" EnableIFrame="true" runat="server" IconUrl="~/images/16/11.png" 
       CloseAction="HidePostBack" EnableConfirmOnClose="true"  IFrameUrl="about:blank" EnableMaximize="true" EnableResize="true"
       Target="Top"  IsModal="true"  Height="630px" Width="700px" >
    </x:Window>

          <x:Window ID="winEditReco" Title="Recomendación" Popup="false" EnableIFrame="true" runat="server" Icon="UserBrown"
        CloseAction="HidePostBack" EnableConfirmOnClose="true" IFrameUrl="about:blank" EnableMaximize="false" EnableResize="false"
        Target="Top" OnClose="winEditReco_Close" IsModal="True" Width="600px" Height="410px">
    </x:Window>

    <x:Window ID="winEditRestri" Title="Restricción" Popup="false" EnableIFrame="true" runat="server" Icon="UserBrown"
        CloseAction="HidePostBack" EnableConfirmOnClose="true" IFrameUrl="about:blank" EnableMaximize="false" EnableResize="false"
        Target="Top" OnClose="winEditRestri_Close" IsModal="True" Width="600px" Height="410px">
    </x:Window>

         
    </form>
      
</body>
</html>