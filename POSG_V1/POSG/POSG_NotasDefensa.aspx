<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="POSG_NotasDefensa.aspx.cs" Inherits="POSG_V1.POSG.POSG_NotasDefensa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 
    <asp:Label ID="lblUser" runat="server" Visible="false"></asp:Label>

   
    <br />
    <div class="alert alert-info text-center"> <i class="fa fa-2x fa-users"></i><h4>FORMULARIO DE REGISTRO DE NOTAS DE LA DEFENSA DE POSGRADOS</h4> </div>

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
    <div class="table-responsive">    
        <asp:GridView ID="tablaActa" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="custom-table">
                 <HeaderStyle BackColor="#03346E" ForeColor="White" Font-Bold="True" HorizontalAlign="Center" />
                <RowStyle BackColor="#ffffff" HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField DataField="strId_act" HeaderText="ID" Visible="false"/>
                <asp:BoundField DataField="strId_per" HeaderText="Cédula" />
                <asp:BoundField DataField="strNomb_per" HeaderText="Nombres" />
                <asp:BoundField DataField="strApellidop_per" HeaderText="Apellido Paterno" />
                <asp:BoundField DataField="strApellidom_per" HeaderText="Apellido Materno" />
                <asp:BoundField DataField="strNombre_Car" HeaderText="Nombre de Maestría" Visible="false" />
                <asp:BoundField DataField="strNombre_per" HeaderText="Periodo Académico" />
                <asp:TemplateField HeaderText="Presenta Documentación">
                    <ItemTemplate><%# Convert.ToBoolean(Eval("bitPresentaDocumentacion_act")) ? "Sí" : "No" %></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="decNotaJurado1_act" HeaderText="Nota jurado 1" />
                <asp:BoundField DataField="decNotaJurado2_act" HeaderText="Nota jurado 2" />
                <asp:BoundField DataField="decNotaJurado3_act" HeaderText="Nota jurado 3" />
                <asp:TemplateField HeaderText="Acción"> 
                    <ItemTemplate >
                         <asp:LinkButton  runat="server" ID="lnkInsertarNotas" CssClass="fa fa-pencil-alt fa-2x" ForeColor="Orange" 
                                        PostBackUrl='<%# "NotasJurados.aspx?idActa=" + Eval("strId_act") %>'>
                           
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>



            </Columns>
        </asp:GridView>
    </div>
 


 

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
    </script>
</asp:Content>
