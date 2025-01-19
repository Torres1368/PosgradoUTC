<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notas.aspx.cs" Inherits="POSG_V1.Pages.Notas" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- No es necesario volver a importar jQuery o DataTables aquí -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="text-center">NOTAS</h1>
    <br />
    <div class="input-group" id="barra-busqueda">
        <div class="form-outline">
            <asp:DropDownList ID="ddlPeriodoAcademico" runat="server" CssClass="form-control">
                <asp:ListItem Text="Selecciona el Período Académico" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-outline">
            <asp:DropDownList ID="ddlNombreMaestria" runat="server" CssClass="form-control">
                <asp:ListItem Text="Selecciona la maestria" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="input-group-append">
            <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClientClick="return validarFiltros();" OnClick="btnFiltrar_Click" />

        </div>
    </div>
    <br />
    <div class="table-responsive">
        <asp:GridView ID="tablaNota" runat="server" AutoGenerateColumns="false" CssClass="display nowrap dataTable dtr-inline custom-table">
            <Columns >
                <asp:BoundField DataField="strId_not" HeaderText="ID" />
                <asp:BoundField DataField="strNomb_per" HeaderText="Nombres" />
                <asp:BoundField DataField="strApellidop_per" HeaderText="Apellido Paterno" />
                <asp:BoundField DataField="strApellidom_per" HeaderText="Apellido Materno" />
                <asp:BoundField DataField="decFormativa_not" HeaderText="Formativa" />
                <asp:BoundField DataField="decFinal_not" HeaderText="Final" />
                <asp:BoundField DataField="decNotaFinal_not" HeaderText="Nota Final Total" />
                <asp:BoundField DataField="decPromedio_not" HeaderText="Promedio" />
                <asp:BoundField DataField="strPeriodoAcademico_not" HeaderText="Periodo Académico" />

            </Columns>
        </asp:GridView>
    </div>
        <script>
    function validarFiltros() {
        var ddlPeriodoAcademico = document.getElementById('<%= ddlPeriodoAcademico.ClientID %>');
        var ddlNombreMaestria = document.getElementById('<%= ddlNombreMaestria.ClientID %>');

        if (ddlPeriodoAcademico.value === '' || ddlNombreMaestria.value === '') {
            alert('Por favor, selecciona un Período Académico y una Maestría');
            return false; 
        }

        return true; 
    }
</script>
</asp:Content>
