<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NotasJurados.aspx.cs" Inherits="POSG_V1.POSG.NotasJurados" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <asp:Label ID="lblUser" runat="server" Visible="false"></asp:Label>
    <br />
        <div class="alert alert-success text-center"><h4><i class="fa fa-1x fa-pencil-alt"></i> Actualizar Notas de Jurados</h4> </div>

    <asp:HiddenField ID="hdnIdActa" runat="server" />
    <div class="alert alert-info ">
        <label class="font-weight-bold" id="lblJurado1" runat="server" >Nota Jurado 1:</label>
        <asp:TextBox ID="txtNotaJurado1" runat="server" CssClass="form-control" oninput="validateInput(this)"></asp:TextBox>
    </div>
    <div class="alert alert-info">
        <label class="font-weight-bold" id="lblJurado2" runat="server">Nota Jurado 2:</label>
        <asp:TextBox ID="txtNotaJurado2" runat="server" CssClass="form-control" oninput="validateInput(this)"></asp:TextBox>
    </div>
    <div class="alert alert-info">
        <label class="font-weight-bold" id="lblJurado3" runat="server">Nota Jurado 3:</label>
        <asp:TextBox ID="txtNotaJurado3" runat="server" CssClass="form-control" oninput="validateInput(this)"></asp:TextBox>
    </div>
<div class="form-group text-center">
    <asp:Button ID="btnConfirmar" runat="server" CssClass="btn btn-warning me-2" Text="Confirmar" OnClick="btnConfirmar_Click" />
     &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClientClick="window.location.href='POSG_NotasDefensa.aspx'; return false;" />
</div>
    <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>


    <script>
        function validateInput(input) {
            const value = input.value; 
            if (!/^\d{1,2}(,\d{0,2})?$/.test(value)) {
                input.setCustomValidity("Solo se permiten números del 1 al 10 con hasta dos decimales utilizando coma. Ej:(7,75)");
            } else {
                const num = parseFloat(value.replace(',', '.')); 
                if (num < 1 || num > 10) {
                    input.setCustomValidity("El valor debe estar entre 1 y 10.");
                } else {
                    input.setCustomValidity("");
                }
            }
            input.reportValidity();
        }
    </script>

    

</asp:Content>