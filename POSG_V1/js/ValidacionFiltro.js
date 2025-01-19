   function validarFiltros() {
       var ddlPeriodoAcademico = document.getElementById('<%= ddlPeriodoAcademico.ClientID %>');
       var ddlNombreMaestria = document.getElementById('<%= ddlNombreMaestria.ClientID %>');

       if (ddlPeriodoAcademico.value === '' || ddlNombreMaestria.value === '') {
           alert('Por favor, selecciona un Período Académico y una Maestría');
           return false; 
       }
       return true; 
   }
