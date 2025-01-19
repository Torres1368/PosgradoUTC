<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="POSG_V1.POSG.Menu" %>
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
                font-size: 60px;
                margin-bottom: 20px;
            }
            
            .menu-option {
                background-color: #f8f9fa;
                border: 2px solid #dee2e6;
                transition: all 0.3s ease;
                cursor: pointer;
            }

            .menu-option:hover {
                background-color: #e9ecef;
                border-color: #ced4da;
                transform: translateY(-5px);
            }

            .menu-option i {
                color: #050c3e;
            }

            .menu-option h4 {
                margin: 0;
                color: #495057;
            }
            h2 {
            text-align: center;
            font-size: 2.5rem;
            font-weight: bold;
            color: #343a40;
            margin-top: 1.5rem;
            margin-bottom: 0.5rem;
            padding: 0.5rem;
            border-bottom: 3px solid  #050c3e;
            background-color: #f8f9fa;
            border-radius: 5px;
            font-family: 'Poppins', sans-serif; 
        }

            h3 {
            text-align: center;
            font-size: 1.5rem;
            font-weight: normal;
            color: #6c757d;
            margin-bottom: 2rem;
            font-family: 'Poppins', sans-serif; 
        }

        .menu-option {
            text-align: center;
            padding: 40px 20px;
            border: 1px solid #ddd;
            border-radius: 10px;
            margin: 10px;
            transition: background-color 0.3s;
            cursor: pointer;
            background-color: #f8f9fa;
            border: 2px solid #dee2e6;
            transition: all 0.3s ease;
        }

           .menu-option:hover {
            background-color: #e9ecef;
            border-color: #ced4da;
            transform: translateY(-5px);
        }

        .menu-option i {
            font-size: 60px;
            color: #050c3e;
            margin-bottom: 20px;
        }

        .menu-option h4 {
            margin: 0;
            color: #495057;
            font-family: 'Poppins', sans-serif;
        }
        .title-container img {
            position: absolute;
            right: 0;
            top: 50%;
            transform: translateY(-50%);
            width: 80px; 
            height: auto;
        }

      .logo {
            width: 400px; 
            height: auto; 
            max-width: 100%; 
            display: block; 
            padding: 10px; 
        }

        .logo-container {
            text-align: left; 
        }
        .menu-option {
    display: flex;
    flex-direction: column;
    justify-content: center; 
    align-items: center; 
    height: 200px; 
    background-color: #ffffff; 
    border-radius: 10px; 
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); 
    text-align: center; 
    transition: transform 0.3s ease, box-shadow 0.3s ease; 
    padding: 20px; 
    margin: 10px 0; 
}

    .menu-option:hover {
        transform: translateY(-5px);
        box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2); 
    }

    .menu-option i {
        margin-bottom: 10px; 
    }

    .menu-option h4 {
        font-size: 1.1rem;
        font-weight: bold; 
        color: #333333; 
    }


    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
          <div>
                <h2 >UNIVERSIDAD TÉCNICA DE COTOPAXI</h2>
                <h3>Seleccione una opción para continuar</h3>
            </div>
        <div class="row justify-content-center">
            <div class="col-md-3 mb-4">
                <div class="menu-option text-center p-4 rounded shadow-sm" onclick="location.href='POSG_DetalleIns.aspx'">
                    <i class="fa fa-graduation-cap fa-3x mb-2"></i>
                    <h4 class="text-uppercase font-weight-bold">Estudiante</h4>
                </div>
            </div>
            <div class="col-md-3 mb-4">
                <div class="menu-option text-center p-4 rounded shadow-sm" onclick="location.href='POSG_Docente1.aspx'">
                    <i class="fa fa-user fa-3x mb-2"></i>
                    <h4 class="text-uppercase font-weight-bold">Docente</h4>
                </div>
            </div>
            <div class="col-md-3 mb-4">
                <div class="menu-option text-center p-4 rounded shadow-sm" onclick="location.href='POSG_ListadoIns.aspx'">
                    <i class="fa fa-users fa-3x mb-2"></i>
                    <h4 class="text-uppercase font-weight-bold">Secretaria Matriz de Inscritos</h4>
                </div>
            </div>
            <div class="col-md-3 mb-4">
                <div class="menu-option text-center p-4 rounded shadow-sm" onclick="location.href='POSG_Inscripciones.aspx'">
                    <i class="fa fa-user-secret fa-3x mb-2"></i>
                    <h4 class="text-uppercase font-weight-bold">Secretario Coordinador</h4>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
