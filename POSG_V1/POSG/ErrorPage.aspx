<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="POSG_V1.POSG.ErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style>
        .menu-option {
            text-align: center;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 10px;
            margin: 10px;
            transition: background-color 0.3s;
            cursor: pointer;
        }

            .menu-option:hover {
                background-color: #f0f0f0;
            }

            .menu-option i {
                font-size: 50px;
                margin-bottom: 10px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="alert alert-danger"><b>NO TIENE PERMISO PARA ESTE PROCESO <i class="fa fa-stop"></i></b></div>
    <div class="container">
        <h2 class="text-center my-4">Seleccione una opción</h2>
        <div class="row justify-content-center">
            <div class="col-md-12">
                <div class="menu-option" onclick="location.href='Menu.aspx'">
                    <i class="fa fa-stop-circle"></i>
                    <h4>Menu principal</h4>
                </div>

            </div>
            </div>
        </div>

</asp:Content>
