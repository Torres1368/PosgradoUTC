<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="POSG_NOTAS.aspx.cs" Inherits="POSG_V1.POSG.POSG_NOTAS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblUser" runat="server" Visible="false"></asp:Label>

    <br />
    <div class="alert alert-info text-center" > <h4><i class="fa fa-2x fa-graduation-cap"></i>PROMEDIO DE LAS NOTAS DE LOS ESTUDIANTES</h4> </div>
    <div class="input-group" id="barra-busqueda">
        <div class="col-sm">
            <asp:DropDownList ID="ddlPeriodoAcademico" runat="server" CssClass="form-control">
                <asp:ListItem Text="Selecciona el Período Académico" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm">
            <asp:DropDownList ID="ddlNombreMaestria" runat="server" CssClass="form-control">
                <asp:ListItem Text="Selecciona la maestria" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm">
            <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClientClick="return validarFiltros();" OnClick="btnFiltrar_Click" />

        </div>

    </div>

                <div class="p-3">
    <asp:Button ID="btnGenerarPDF" Text="GENERAR REPORTE PDF"  CssClass="btn btn-danger" runat="server" OnClick="btnGenerarPDF_Click"  /> 
        </div>
    <div class="table-responsive">
        <asp:GridView ID="tablaNota" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="custom-table">
            <HeaderStyle BackColor="#03346E" ForeColor="White" Font-Bold="True" HorizontalAlign="Center" />
                <RowStyle BackColor="#ffffff" HorizontalAlign="Center" />

            <Columns >
                <asp:BoundField DataField="strId_not" HeaderText="ID" Visible="false"/>
                <asp:BoundField DataField="strId_per" HeaderText="Cédula" />
                <asp:BoundField DataField="strNomb_per" HeaderText="Nombres" />
                <asp:BoundField DataField="strApellidop_per" HeaderText="Apellido Paterno" />
                <asp:BoundField DataField="strApellidom_per" HeaderText="Apellido Materno" />
                <asp:BoundField DataField="decPromedioCurricular_not" HeaderText="Promedio Curricular" />
                <asp:BoundField DataField="decPromedioDefensa_not" HeaderText="Promedio Defensa" />
                <asp:BoundField DataField="decTotalActa_not" HeaderText="Nota Final acta" />
                <asp:BoundField DataField="strNombre_per" HeaderText="Periodo Académico" />
 

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
            
            return false ;
        }


        return true; 
    }
</script>
</asp:Content>
