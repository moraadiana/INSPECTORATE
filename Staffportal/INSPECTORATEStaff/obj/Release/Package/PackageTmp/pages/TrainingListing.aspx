<%@ Page Title="" Language="C#" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" EnableEventValidation="false"  CodeBehind="TrainingListing.aspx.cs" Inherits="INSPECTORATEStaff.pages.TrainingListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
     <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Trainings Available
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Training Listings</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <asp:MultiView ID="mlvTrainings" runat="server">
                <asp:View ID="vwAvailableTrainings" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-server"></i> Available Trainings</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="col-md-12">
                                <p class="text-center">
                                    <%--<a class="" href="<% =viewApplicationsTable() %>">  </a>--%>
                                  
                                    <asp:Button CssClass="btn btn-pill btn-warning u-posRelative pull-right" ID="btnViewMyApps" runat="server" Text="View My Applications" OnClick="btnViewMyApps_Click" />

                                </p>
                                
                            </div>
                            <br/>
                            <br/>
                            <br/>
                  
                            <!-- /.table-responsive -->
                            <div class="col-md-12">
                                <div class="DivToPrint" runat="server"></div>
                                <div class="table-responsive">
                                <asp:GridView ID="listCourse" runat="server" AutoGenerateColumns="False" CssClass="table no-margin table-bordered table-striped" DataKeyNames="Course Code" OnRowCommand="listCourse_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="Course Code" HeaderText="Course" />
                                        <asp:BoundField DataField="Course Tittle" HeaderText="Course" />
                                        <asp:BoundField DataField="Provider" HeaderText="Trainer" />
                                        <asp:BoundField DataField="Start Date" HeaderText="Start Date" />
                                        <asp:BoundField DataField="End Date" HeaderText="End Date" />
                                        <asp:BoundField DataField="Location" HeaderText="Venue" />
                                        <%--<asp:BoundField DataField="Duration" HeaderText="Duration" />--%>
                                        <asp:BoundField DataField="Closed" HeaderText="Status" />
                                        <asp:ButtonField ButtonType="Button" CommandName="apply" Text="Apply">
                                        <ControlStyle CssClass="btn btn-success" />
                                        </asp:ButtonField>
                                    </Columns>
                                </asp:GridView>
                                </div>
                                    
                            </div>
                            <%--End Table Responsive--%>

                        </div>
                      
                    </div>
                </div>
                <!-- /.col -->
            </div>
                </asp:View>
            
                <asp:View ID="vwMyTraininigs" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-server"></i> My Applications</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                          
                            <br/>
                            <br/>
                            <br/>
                  
                            <!-- /.table-responsive -->
                            <div class="col-md-12">
                                <div class="table-responsive">
                                <asp:GridView ID="myApplications" runat="server" AutoGenerateColumns="False" CssClass="table no-margin table-bordered table-striped" >
                                    <Columns>
                                        <asp:BoundField DataField="Application No" HeaderText="Applicatoin No." />
                                        <asp:BoundField DataField="Course Title" HeaderText="Course" />
                                        <asp:BoundField DataField="Application Date" HeaderText="Application Date" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" /> 
                                    </Columns>
                                </asp:GridView>
                                </div>
                            </div>
                            <%--End Table Responsive --%>

                        </div>
                      
                    </div>
                </div>
                <!-- /.col -->
            </div>
                    </asp:View>
            </asp:MultiView>
            <%--END MULTIVIEW--%>
      
        <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
     </div>
     </div>
     </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="customJS" runat="server">
    <script>
        /*FOR TRAINING LIST DATATABLE*/
        $(document).ready(function () {
            $('#<%=listCourse.ClientID%>').DataTable()
            
        });

        $(document).ready(function () {
            $('#<%=myApplications.ClientID%>').DataTable()

        });
    </script>
  

    </asp:Content>

