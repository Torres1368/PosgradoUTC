<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="POSG_NotasEstudiantes.aspx.cs" Inherits="POSG_V1.POSG.POSG_NotasEstudiantes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblUser" runat="server" Visible="false"></asp:Label>

    <br />
        <div class="alert alert-info text-center"> <h4>NOTAS DEL ESTUDIANTE</h4> </div>
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
            <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="btnFiltrar_Click" OnClientClick="return validarFiltros();" />

        </div>
    </div>

    <br />
    <div class="table-responsive">
   <asp:GridView ID="tablaNota" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="table table-bordered">
    <HeaderStyle BackColor="#03346E" ForeColor="White" Font-Bold="True" HorizontalAlign="Center" />
    <RowStyle BackColor="#ffffff" HorizontalAlign="Center" />

    <Columns>
        <asp:BoundField DataField="strId_not" HeaderText="ID" Visible="false" />
        <asp:BoundField DataField="strId_per" HeaderText="Cédula" />
        <asp:BoundField DataField="strNomb_per" HeaderText="Nombres" />
        <asp:BoundField DataField="strApellidop_per" HeaderText="Apellido Paterno" />
        <asp:BoundField DataField="strApellidom_per" HeaderText="Apellido Materno" />
        <asp:BoundField DataField="strNombre_Car" HeaderText="Nombre de la Maestría" />
        <asp:BoundField DataField="decPromedioCurricular_not" HeaderText="Promedio Curricular" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="decPromedioDefensa_not" HeaderText="Promedio Defensa" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="decTotalActa_not" HeaderText="Total Acta" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="strNombre_per" HeaderText="Período Académico" />
        
        <asp:TemplateField HeaderText="Actas Firmadas">
            <ItemTemplate>
                <asp:PlaceHolder ID="ActasFirmadas" runat="server">
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
                    ID="lblNoDisponible1" 
                    runat="server" 
                    Text="No disponible" 
                    ForeColor="Red" 
                    Visible='<%# Eval("strUrlActasFirmadas_act") == DBNull.Value || string.IsNullOrEmpty(Eval("strUrlActasFirmadas_act").ToString()) %>' 
                />
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
