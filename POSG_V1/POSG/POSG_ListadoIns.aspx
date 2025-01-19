<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="POSG_ListadoIns.aspx.cs" Inherits="POSG_V1.POSG.POSG_ListadoIns" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .modal-dialog-wide {
        max-width: 90%; /* Ajustar el porcentaje según sea necesario */
    }
         .true-background {
        background-color: green;
        color: white;
    }

    .false-background {
        background-color: red;
        color: white;
    }
        .gridview-header {
            background-color: #0e407f;
            font-weight: bold;
            color: white;
            text-align: center;
        }

        .gridview-row {
            background-color: #ffffff;
            text-align: center;
        }

        .gridview-row-alternate {
            background-color: #EAEAD3;
        }

        .gridview-selected-row {
            background-color: DodgerBlue;
            font-weight: bold;
            color: white;
        }

        .gridview-pager {
            background-color: #F7F7DE;
            color: black;
            text-align: right;
        }

        .gridview-footer {
            background-color: #CCCC99;
        }

        .gridview-sorted-ascending {
            background-color: #FBFBF2;
        }

        .gridview-sorted-descending {
            background-color: #575357;
            color: white;
        }
         .text-justify {
        text-align: justify;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <asp:Label ID="lblUser" runat="server" Visible="false"></asp:Label>
    <div class="container-fluid">
        <div class="alert alert-primary"><i class="fa fa-2x fa-graduation-cap">&nbsp; Detalle inscritos para el proceso de titulación </i></div>
        
        <div class="row">
            <div class="col-sm">
                <asp:DropDownList ID="ddlSedes" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSedes_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="col-sm">
                <asp:DropDownList ID="ddlFacultad" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFacultad_SelectedIndexChanged" runat="server"></asp:DropDownList>
            </div>
            <div class="col-sm">
                <asp:DropDownList ID="ddlMaestria" AutoPostBack="true" OnSelectedIndexChanged="ddlMaestria_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
            <div class="col-sm" hidden>
                <asp:DropDownList ID="ddlCohorte" AutoPostBack="true" OnSelectedIndexChanged="ddlCohorte_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div><br /></div>
        <asp:Label ID="lblMsg" runat="server" Visible="true" ForeColor="Black" CssClass="form-control alert alert-info"></asp:Label>
        <div runat="server" id="divDetalle">
        <div class="alert alert-success"><b>LISTADO DE INSCRITOS EN EL PROCESO DE TITULACION DE LA :</b> &nbsp;<asp:Label ID="lblMaestria" runat="server" ></asp:Label> </div>
         <asp:Button ID="btnReport" OnClick="btnReport_Click" CssClass="btn btn-info btn-lg text-right" runat="server" Text="Descargar Reporte" />
            <hr />
        <asp:GridView ID="gvwMatriculados" Caption="se han encontrado:"
            CaptionAlign="Top" Width="100%" AutoGenerateColumns="false" runat="server" 
            OnSelectedIndexChanged="gvwMatriculados_SelectedIndexChanged" OnRowCommand="gvwMatriculados_RowCommand"
            DataKeyNames="strid_ins,strCod_per,strcod_matric,strid_per,strid_per2,strObs1_ins">
            <Columns>
                <asp:BoundField DataField="strid_ins" Visible="false" HeaderText="Codigo"></asp:BoundField>
                <asp:BoundField DataField="strCod_per" Visible="false" HeaderText="Periodo"></asp:BoundField>
                <asp:BoundField DataField="strcod_matric" Visible="false" HeaderText="MC"></asp:BoundField>
                <asp:BoundField DataField="strid_per" HeaderText="Cedula"></asp:BoundField>
                <asp:BoundField DataField="strUserLog" HeaderText="Nombres y Apellidos"></asp:BoundField>

                <asp:TemplateField HeaderText="Enlace al documento">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkDocumento" runat="server" NavigateUrl='<%# Eval("strUrl_ins") %>' Text="Ver documento" Target="_blank"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="strObs1_ins" HeaderText="Tutor"></asp:BoundField>
                <asp:TemplateField HeaderText="Revisada">
                    <ItemTemplate>
                        <asp:Label ID="lblRevisada" runat="server" Text='<%# Convert.ToBoolean(Eval("bitMatrizRevisada_ins")) ? "✔" : "✘" %>'
                            CssClass='<%# Convert.ToBoolean(Eval("bitMatrizRevisada_ins")) ? "true-background" : "false-background" %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Pagada">
                    <ItemTemplate>
                        <asp:Label ID="lblPagada" runat="server" Text='<%# Convert.ToBoolean(Eval("bitPagado_ins")) ? "✔" : "✘" %>'
                            CssClass='<%# Convert.ToBoolean(Eval("bitPagado_ins")) ? "true-background" : "false-background" %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>



                <asp:TemplateField HeaderText="Aprobada">
                    <ItemTemplate>
                        <asp:Label ID="lblAprobada" runat="server" Text='<%# Convert.ToBoolean(Eval("bitMatrizAprobada_ins")) ? "✔" : "✘" %>'
                            CssClass='<%# Convert.ToBoolean(Eval("bitMatrizAprobada_ins")) ? "true-background" : "false-background" %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ShowHeader="False">
                    <HeaderTemplate>
                        Seleccionar
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnSelect" CssClass="fa fa-hand-o-up fa-2x" ForeColor="Orange" runat="server" ToolTip="Ver detalles de inscripción" CausesValidation="False" CommandName="Select" Text=""></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Font-Bold="False" Font-Italic="True" HorizontalAlign="Center" />
                </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False">
                                    <HeaderTemplate>Validar</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnValidar" CssClass="fa fa-hand-o-up fa-2x" ForeColor="Blue" runat="server"
                                            CausesValidation="False" CommandName="AprobarAll" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text=""></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="True" ForeColor="#337AB7" HorizontalAlign="Center" />
                                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="gridview-header" />
            <RowStyle CssClass="gridview-row" />
            
            <SelectedRowStyle CssClass="gridview-selected-row" />
            <PagerStyle CssClass="gridview-pager" />
            <FooterStyle CssClass="gridview-footer" />
            <SortedAscendingCellStyle CssClass="gridview-sorted-ascending" />
            <SortedDescendingCellStyle CssClass="gridview-sorted-descending" />
        </asp:GridView>
         <%-- <div class="panel panel-primary">

                <div class="panel-heading">
                    <div class="alert alert-info"><i class="fa fa-2x fa-money-bill-wave"></i> Pagos</div>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                       
                        
                        

                    </div>
                   
                </div>

                <div class="panel-footer panel-primary">
                </div>
            </div>--%>



       <div class="panel panel-primary">

                <div class="panel-heading">
                    <div class="alert alert-info"><i class="fa fa-2x fa-archive"></i> Objetivos</div>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <%--<asp:Button ID="btnAddObj" runat="server" Text="Agregar Objetivo" CssClass="btn btn-primary" OnClientClick="$('#myModal').modal('show'); return false;" />--%>
                        
                        <div>
                            <br />
                        </div>
                        
                        <asp:GridView ID="gvwObjetivos" runat="server" 
                            Caption="se han encontrado:" CaptionAlign="Top" Width="100%" AutoGenerateColumns="false"
                            DataKeyNames="strId_insd,strId_ins,strTipo_insd,strDescTipo_insd" >
                            <Columns>
                                <asp:BoundField DataField="strid_insd" Visible="false" HeaderText="Codigo"></asp:BoundField>
                                <asp:BoundField DataField="strid_ins" Visible="false" HeaderText="Periodo"></asp:BoundField>
                                <asp:BoundField DataField="strObs1_insd" Visible="true" HeaderText="TIPO DE OBJETIVO">
                                    <ItemStyle CssClass="text-justify" />
                                </asp:BoundField>
                                <asp:BoundField DataField="strDescTipo_insd" HeaderText="Descripcion">
                                    <ItemStyle CssClass="text-justify" />
                                </asp:BoundField>
                                <asp:BoundField DataField="strObsAprobado_insd" HeaderText="Observaciones">
                                    <ItemStyle CssClass="text-justify" />
                                </asp:BoundField>

                                <asp:TemplateField HeaderText="Aprobado">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAprobada" runat="server" Text='<%# Convert.ToBoolean(Eval("bitaprobado_insd")) ? "✔" : "✘" %>'
                                            CssClass='<%# Convert.ToBoolean(Eval("bitaprobado_insd")) ? "true-background" : "false-background" %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                              
                            </Columns>
                            <HeaderStyle CssClass="gridview-header" />
                            <RowStyle CssClass="gridview-row" />
                            <SelectedRowStyle CssClass="gridview-selected-row" />
                            <PagerStyle CssClass="gridview-pager" />
                            <FooterStyle CssClass="gridview-footer" />
                            <SortedAscendingCellStyle CssClass="gridview-sorted-ascending" />
                            <SortedDescendingCellStyle CssClass="gridview-sorted-descending" />
                        </asp:GridView>

                    </div>
                   
                </div>

                <div class="panel-footer panel-primary">
                </div>
            </div>

        <div class="panel panel-primary">
                  <hr />
              <%--  <asp:Button ID="btnAddTribunal" runat="server" Text="Agregar Tribunal" CssClass="btn btn-primary" />--%>
                  <hr />   
                <div class="panel-heading">
                    <div class="alert alert-success"><i class="fa fa-2x fa-group"></i> Tribunal </div>
                </div>
                <div class="panel-body">
                   
                    <div class="form-group">
                         <%--<asp:Button ID="btnAddObj" runat="server" Text="Agregar Objetivo" CssClass="btn btn-primary" OnClientClick="$('#myModal').modal('show'); return false;" />--%>
                        
                        <asp:GridView ID="gvwTribunal" runat="server" Caption="se han encontrado:"
                            CaptionAlign="Top" Width="100%" AutoGenerateColumns="false"
                            DataKeyNames="strId_insd,strId_ins">
                            <Columns>
                                <asp:BoundField DataField="strid_insd" Visible="false" HeaderText="Codigo"></asp:BoundField>
                                <asp:BoundField DataField="strid_ins" Visible="false" HeaderText="Periodo"></asp:BoundField>
                                <asp:BoundField DataField="strDescTipo_insd" Visible="true" HeaderText="Tribunal"></asp:BoundField>
                                <asp:BoundField DataField="strCedTribunal_insd" HeaderText="CEDULA"></asp:BoundField>
                                <asp:BoundField DataField="strObs2_insd" HeaderText="Nombres y apellidos"></asp:BoundField>
                                  <asp:TemplateField HeaderText="Aprobada">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAprobada" runat="server" Text='<%# Convert.ToBoolean(Eval("bitaprobado_insd")) ? "✔" : "✘" %>'
                                            CssClass='<%# Convert.ToBoolean(Eval("bitaprobado_insd")) ? "true-background" : "false-background" %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridview-header" />
                            <RowStyle CssClass="gridview-row" />
                            <SelectedRowStyle CssClass="gridview-selected-row" />
                            <PagerStyle CssClass="gridview-pager" />
                            <FooterStyle CssClass="gridview-footer" />
                            <SortedAscendingCellStyle CssClass="gridview-sorted-ascending" />
                            <SortedDescendingCellStyle CssClass="gridview-sorted-descending" />
                        </asp:GridView>  
                    </div>
                </div>

                <div class="panel-footer panel-primary">
                </div>
            </div>






        </div>

        <%--modal--%>
        <div class="modal fade" id="modalReport" tabindex="-1" role="dialog" aria-labelledby="modalReportLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalReportLabel">Reporte</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                        <asp:ObjectDataSource ID="odsRpt" runat="server" SelectMethod="LoadPOSG_INSCRIPCION" TypeName="ClassLibraryTesis.POSG_INSCRIPCION">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="xReport" Name="comodin" Type="String" />
                                <asp:ControlParameter ControlID="ddlCohorte" Name="filtro1" PropertyName="SelectedValue" Type="String" />
                                <asp:Parameter DefaultValue="sd" Name="filtro2" Type="String" />
                                <asp:Parameter DefaultValue="sd" Name="filtro3" Type="String" />
                                <asp:Parameter DefaultValue="sd" Name="filtro4" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <rsweb:ReportViewer ID="rvwRpt" runat="server" Font-Names="Verdana" Width="100%" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                            <LocalReport ReportPath="POSG\reports\rptMatriz.rdlc">
                                <DataSources>
                                    <rsweb:ReportDataSource DataSourceId="odsRpt" Name="DataSetInscripcion" />
                                </DataSources>
                            </LocalReport>
                        </rsweb:ReportViewer>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnGuardarObjetivo" runat="server"  Text="Guardar" CssClass="btn btn-primary" Visible="false" />

                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                        <%--<asp:Button ID="btnCerrarModal" runat="server" Text="Cerrar" CssClass="btn btn-secondary" />--%>

                    </div>
                </div>
            </div>
        </div>
        <%--modal--%>

           <%--modal tribunal--%>
             <div class="modal fade" id="modalValidar" tabindex="-1" role="dialog" aria-labelledby="modalValidarLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalValidarLabel">Validar</h5>
                       
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblMsgT" runat="server" CssClass="alert-danger"></asp:Label>
                        <div class="form-group">

                            <label for="chkPagado">Valores pagados:</label>
                            <asp:CheckBox ID="chkPagado" runat="server" />

                            <div>
                                <br />
                            </div>
                            <label for="chkRevisado">Matriz revisada:</label>
                            <asp:CheckBox ID="chkRevisado" runat="server" />

                            <div>
                                <br />
                            </div>
                            <label for="chkPagado">matriz aprobada :</label>
                            <asp:CheckBox ID="chkAprobada" runat="server" />

                            <div>
                                <br />
                            </div>
                            
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAddT" runat="server" required="true" Text="Guardar" CssClass="btn btn-primary"  OnClick="btnAddT_Click" />
                        
                        <%--<asp:Button ID="btnGuardarObjetivo" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarObjetivo_Click" />--%>
                       <%-- <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>--%>
                        <asp:Button ID="btnCloseTrib"  runat="server" Text="Cerrar" CssClass="btn btn-secondary" OnClick="btnCloseTrib_Click"  />
          
                    </div>
                    <%--<asp:RequiredFieldValidator ID="rfvbtnAddT" ControlToValidate="btnAddT" runat="server" ErrorMessage="campo obligatorio" CssClass="alert alert-danger" ValidationGroup="tribunal"></asp:RequiredFieldValidator>--%>
                </div>
            </div>
        </div>

         <%--modal validar--%>

    </div>
</asp:Content>