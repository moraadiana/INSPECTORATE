<%@ Page Title="" Language="C#" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="ImprestSurrenderList.aspx.cs" Inherits="Appkings.pages.ImprestSurrenderList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Imprest Requisitions
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Imprest Accounting</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-warning">
                        <div class="box-header with-border">
                            <h3 class="box-title">My Imprest Accounting</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <p class="text-center"><a class="btn btn-pill btn-warning u-posRelative" href="ImprestAccounting.aspx?Tp=new">New Imprest Surrender<span class="waves"></span> </a></p>
                            <br />
                            <div class="table-responsive">
                                <table id="example1" class="table no-margin">
                                    <thead>
                                        <tr>
                                            <th class="small">#NO</th>
                                            <%--<th class="small">Advance Type</th>--%>
                                            <th class="small">Account No</th>
                                            <%--<th class="small">Account Name</th>--%>
                                            <th class="small">Amount</th>
                                            <th class="small">Status</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%=LoadImprests()%>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>