﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="POSG_ActasGrado.aspx.cs" Inherits="POSG_V1.POSG.POSG_ActasGrado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style>
        .hidden-iframe {
            display: none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <br />
     <asp:Label ID="lblUser" runat="server" Visible="false"></asp:Label>
    <div class="alert alert-info text-center"> <h4><i class="fa fa-2x fa-graduation-cap"></i> FORMULARIO DE LAS ACTAS DE GRADO</h4> </div>
    <div class="input-group" id="barra-busqueda">
        <div class="col-sm">
            <asp:DropDownList ID="ddlPeriodoAcademico" runat="server" CssClass="form-control">
                <asp:ListItem Text="Selecciona el Período Académico" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm">
            <asp:DropDownList ID="ddlNombreMaestria" runat="server" CssClass="form-control">
                <asp:ListItem Text="Selecciona la maestría" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm">
            <asp:DropDownList ID="ddlPresentaDocumentacion" runat="server" CssClass="form-control">
                <asp:ListItem Text="Selecciona si presenta documentación" Value=""></asp:ListItem>
                <asp:ListItem Text="Sí" Value="True"></asp:ListItem>
                <asp:ListItem Text="No" Value="False"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm">
            <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClientClick="return validarFiltros();" OnClick="btnFiltrar_Click" />
        </div>
    </div>
    <br />
<div class="p-3 border alert alert-secondary">
    <h4>FORMULARIO PARA GENERAR CERTIFICADOS</h4>
    <input type="date" id="fechaSeleccionada" name="fechaSeleccionada" class="form-control" style="display: inline-block; width: auto; margin-right: 10px;" />
  <asp:Button ID="btnGenerarCertificado" OnClientClick="showLoadingAlert()" CssClass="btn btn-danger" runat="server" Text="Generar Certificados" OnClick="btnGenerarCertificado_Click" />

    <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>
</div>



<div class="table-responsive">
    <asp:GridView ID="tablaActa" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="custom-table">
            <HeaderStyle BackColor="#03346E" ForeColor="White" Font-Bold="True" HorizontalAlign="Center" />
                <RowStyle BackColor="#ffffff" HorizontalAlign="Center" />
        <Columns>
            <asp:BoundField DataField="strId_act" HeaderText="ID"   Visible="false"/>
            <asp:BoundField DataField="strId_per" HeaderText="Cédula" />
            <asp:BoundField DataField="strNomb_per" HeaderText="Nombres" />
            <asp:BoundField DataField="strApellidop_per" HeaderText="Apellido Paterno" />
            <asp:BoundField DataField="strApellidom_per" HeaderText="Apellido Materno" />
            <asp:BoundField DataField="strNombre_Car" HeaderText="Nombre de Maestría" />
            <asp:BoundField DataField="strNombre_per" HeaderText="Periodo Académico" Visible="false"/>
            <asp:TemplateField HeaderText="Presenta Documentación">
                <ItemTemplate>
                    <%# Convert.ToBoolean(Eval("bitPresentaDocumentacion_act")) ? "Sí" : "No" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Actas sin firmar">
                    <ItemTemplate>
                        <asp:PlaceHolder ID="phLinkCertificado1" runat="server">
                            <asp:HyperLink 
                                CssClass="fa fa-file-alt fa-2x" 
                                ForeColor="Red"  
                                ID="lnkCertificado" 
                                runat="server" 
                                NavigateUrl='<%# Eval("strUrlActasNoFirmadas_act") != DBNull.Value && !string.IsNullOrEmpty(Eval("strUrlActasNoFirmadas_act").ToString()) 
                                    ? ResolveUrl(Eval("strUrlActasNoFirmadas_act").ToString()) + "?t=" + DateTime.Now.Ticks 
                                    : "#" %>'
                                Target="_blank" 
                                Visible='<%# Eval("strUrlActasNoFirmadas_act") != DBNull.Value && !string.IsNullOrEmpty(Eval("strUrlActasNoFirmadas_act").ToString()) %>' 
                            />
                        </asp:PlaceHolder>
                        <asp:Label 
                            ID="lblNoDisponibleCertificado1" 
                            runat="server" 
                            Text="No disponible" 
                            ForeColor="Red" 
                            Visible='<%# Eval("strUrlActasNoFirmadas_act") == DBNull.Value || string.IsNullOrEmpty(Eval("strUrlActasNoFirmadas_act").ToString()) %>' 
                        />
                    </ItemTemplate>

            </asp:TemplateField>
            
<asp:TemplateField HeaderText="SUBIR ACTA FIRMADA">
    <ItemTemplate>
        <asp:Button ID="btnSubirActaFirmada" runat="server" Text="Subir Acta Firmada" 
                    CommandArgument='<%# Eval("strId_act") %>' 
                    OnClick="btnSubirActaFirmada_Click" 
                    CssClass="btn btn-success" />
    </ItemTemplate>
</asp:TemplateField>


              <asp:TemplateField HeaderText="Actas firmadas">
<ItemTemplate>
    <asp:PlaceHolder ID="phLinkActasFirmadas1" runat="server">
        <asp:HyperLink 
            CssClass="fa fa-file-alt fa-2x" 
            ForeColor="Green"  
            ID="lnkActasFirmadas" 
            runat="server" 
            NavigateUrl='<%# Eval("strUrlActasFirmadas_act") != DBNull.Value && !string.IsNullOrEmpty(Eval("strUrlActasFirmadas_act").ToString()) 
                ? ResolveUrl(Eval("strUrlActasFirmadas_act").ToString()) 
                : "#" %>'
            Target="_blank" 
            Visible='<%# Eval("strUrlActasFirmadas_act") != DBNull.Value && !string.IsNullOrEmpty(Eval("strUrlActasFirmadas_act").ToString()) %>' 
        />
    </asp:PlaceHolder>
    <asp:Label 
        ID="lblNoDisponibleActasFirmadas1" 
        runat="server" 
        Text="Vacio" 
        ForeColor="Red" 
        Visible='<%# Eval("strUrlActasFirmadas_act") == DBNull.Value || string.IsNullOrEmpty(Eval("strUrlActasFirmadas_act").ToString()) %>' 
    />
</ItemTemplate>

            </asp:TemplateField>

        </Columns>
    </asp:GridView>
</div>


 <iframe id="downloadIframe" class="hidden-iframe"></iframe>
    
    <script>
        function validarFiltros() {
            var ddlPeriodoAcademico = document.getElementById('<%= ddlPeriodoAcademico.ClientID %>');
            var ddlNombreMaestria = document.getElementById('<%= ddlNombreMaestria.ClientID %>');

            if (ddlPeriodoAcademico.value === '' || ddlNombreMaestria.value === '') {
                Swal.fire({
                    title: "¡ALERTA!",
                    text: "Debe seleccionar el periodo académico y el nombre de la maestría antes de filtrar.",
                    icon: "warning"
                });
                return false;
            }

            return true;
        }

        function showLoadingAlert() {
            Swal.fire({
                title: 'Generando Actas de grado',
                text: 'Por favor espera mientras se generan los archivos.',
                icon: 'info',
                allowOutsideClick: false,
                showConfirmButton: false,
                willOpen: () => {
                    Swal.showLoading();
                }
            });

            var iframe = document.getElementById('downloadIframe');
            iframe.onload = function () {
                Swal.close();
                window.history.replaceState(null, null, window.location.href + '?download=true');
            };
        }
    </script>

</asp:Content>