<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="POSG_V1.POSG.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

         <!-- CHART js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<!-- dashboard inner -->
<div class="midde_cont">
    <div class="container-fluid">
        <div class="row column_title">
            <div class="col-md-12">
                <div class="page_title">
                    <h2>Gráficas</h2>
                </div>
            </div>
        </div>
        <!-- row -->
        <div class="row column1">
            <div class="col-lg-6">
                <div class="white_shd full margin_bottom_30">
                    <div class="full graph_head">
                        <div class="heading1 margin_0">
                            <h2>Top 5 maestrias con mejor promedio en actas</h2>
                        </div>
                    </div>
                    <div class="map_section padding_infor_info">
                        <canvas id="doughnut_chart"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="white_shd full margin_bottom_30">
                    <div class="full graph_head">
                        <div class="heading1 margin_0">
                            <h2>Top 5 maestrias con mayores inscripciones</h2>
                        </div>
                    </div>
                    <div class="map_section padding_infor_info">
                        <canvas  id="pie_chart" width="300" height="50"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <!-- row for Line Chart -->
        <div class="row column1">
            <div class="col-lg-12">
                <div class="white_shd full margin_bottom_30">
                    <div class="full graph_head">
                        <div class="heading1 margin_0">
                            <h2>Top promedios de Defensa por Maestria y Periodo</h2>
                        </div>
                    </div>
                    <div class="map_section padding_infor_info">
                        <canvas id="defenseChart" width="700" height="200"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <!-- end row -->
    </div>
</div>







                        
 
<script type="text/javascript">


    // Variables para los colores predefinidos
    var predefinedColors = [
        'rgba(255, 87, 51, 0.5)', // Naranja
        'rgba(153, 102, 255, 0.5)', // Púrpura
        'rgba(54, 162, 235, 0.5)',  // Azul
        'rgba(255, 206, 86, 0.5)',  // Amarillo
        'rgba(255, 159, 64, 0.5)'   // Naranja claro
    ];

    // Obtener los datos desde el backend
    var labelsDefense = <%= "['" + LabelsDefense.Replace("\"", "\\\"").Replace(",", "','") + "']" %>;
    var dataDefense = <%= "[" + DataDefense + "]" %>;



    // Crear el gráfico de barras
    var ctx = document.getElementById('defenseChart').getContext('2d');
    var defenseChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labelsDefense,
            datasets: [{
                label: 'Top Promedios de Defensa',
                data: dataDefense,
                backgroundColor: predefinedColors.slice(0, labelsDefense.length),
                borderColor: predefinedColors.slice(0, labelsDefense.length).map(color => color.replace('0.5', '1')), // Ajustar el color del borde
                borderWidth: 1
            }]
        },
        options: {
            animation: false,
            responsive: true,
            // ... opciones del gráfico
        }
    });

</script>

<script type="text/javascript">
    var predefinedColors = [
        'rgba(220, 20, 60, 0.5)',    // Rojo carmesí
        'rgba(106, 90, 205, 0.5)',   // Azul oscuro
        'rgba(255, 215, 0, 0.5)',    // Amarillo oro

        'rgba(95, 158, 160, 0.5)',   // Verde grisáceo
        'rgba(30, 144, 255, 0.5)',   // Azul dodger
        'rgba(255, 99, 132, 0.5)',   // Rojo
        'rgba(100, 255, 218, 0.5)',  // Verde claro
        'rgba(140, 140, 255, 0.5)',  // Azul claro
        'rgba(255, 206, 86, 0.5)',   // Amarillo
        'rgba(75, 192, 192, 0.5)'    // Verde agua
    ];

    var labelsPie = <%= "['" + LabelsPie.Replace("\"", "\\\"").Replace(",", "','") + "']" %>;
    var dataPie = <%= "[" + DataPie + "]" %>;
    var backgroundColors = predefinedColors.slice(0, dataPie.length);


    // Crear el gráfico de pastel
    var ctx = document.getElementById('pie_chart').getContext('2d');
    var pieChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labelsPie,
            datasets: [{
                data: dataPie,
                backgroundColor: backgroundColors,
                borderColor: backgroundColors.map(color => color.replace('0.5', '1')), // Ajustar el color del borde
                borderWidth: 1
            }]
        },
        options: {
            // ... opciones del gráfico
        }
    });
</script>

<script type="text/javascript">
    var predefinedColors = [
        'rgba(0, 128, 128, 0.5)',    // Verde oscuro
        'rgba(255, 69, 0, 0.5)',     // Rojo anaranjado
        'rgba(30, 144, 255, 0.5)',   // Azul dodger
        'rgba(218, 165, 32, 0.5)',   // Dorado
        'rgba(106, 90, 205, 0.5)',   // Azul oscuro
        'rgba(255, 215, 0, 0.5)',    // Amarillo oro
        'rgba(95, 158, 160, 0.5)',   // Verde grisáceo
        'rgba(64, 224, 208, 0.5)',   // Turquesa
        'rgba(220, 20, 60, 0.5)',    // Rojo carmesí
        'rgba(47, 79, 79, 0.5)',     // Gris oscuro
        'rgba(75, 192, 192, 0.5)',  // Verde agua
        'rgba(255, 99, 132, 0.5)',  // Rojo
        'rgba(54, 162, 235, 0.5)',  // Azul
        'rgba(255, 206, 86, 0.5)',  // Amarillo
        'rgba(153, 102, 255, 0.5)', // Púrpura
        'rgba(255, 159, 64, 0.5)',  // Naranja
        'rgba(100, 255, 218, 0.5)', // Verde claro
        'rgba(200, 99, 255, 0.5)',  // Lila
        'rgba(255, 140, 100, 0.5)'  // Coral
    ];

    var labelsDoughnut = <%= "['" + LabelsDoughnut.Replace("\"", "\\\"").Replace(",", "','") + "']" %>;
    var dataDoughnut = <%= "[" + DataDoughnut + "]" %>;
    var backgroundColors = predefinedColors.slice(0, dataDoughnut.length);



    // Crear el gráfico de dona
    var ctx = document.getElementById('doughnut_chart').getContext('2d');
    var doughnutChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labelsDoughnut,
            datasets: [{
                data: dataDoughnut,
                backgroundColor: backgroundColors,
                borderColor: backgroundColors.map(color => color.replace('0.5', '1')), // Ajustar el color del borde
                borderWidth: 1
            }]
        },
        options: {
            // ... opciones del gráfico
        }
    });
</script>






</asp:Content>