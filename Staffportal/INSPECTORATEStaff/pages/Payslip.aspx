<%@ Page Language="C#" Title="Payslip" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="Payslip.aspx.cs" Inherits="INSPECTORATEStaff.pages.Payslip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Payslip
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Payslip</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <h3 class="box-title">Individual payslip</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <div class="btn-group">
                                    <button type="button" class="btn btn-box-tool dropdown-toggle" data-toggle="dropdown">
                                        <i class="fa fa-wrench"></i>
                                    </button>
                                    <ul class="dropdown-menu" role="menu">
                                    </ul>
                                </div>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-2 control-label">Year:</label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="ddlYear" class="form-control select2" runat="server" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                    <label for="inputEmail3" class="col-sm-2 control-label">Month</label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="DdMonth" class="form-control select2" runat="server" OnSelectedIndexChanged="DdMonth_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                    <div class="col-sm-2">
                                    <div class="col-sm-2">
                                        <button type="button" onclick="lbtnViewpayslip_Click" id="lbtnViewpayslip_Click" class="btn btn-block btn-primary"><i class="fa fa-file-pdf-o"></i>View payslip</button>
                                    </div>
                                </div>
                            </div>
                            <span style="font-size: 10pt">
                                <iframe runat="server" id="myPDF" src="" width="100%" height="500" />
                            </span>
                        </div>
                    </div>
                    <!-- /.box -->
                </div>
                <!-- /.col -->
            </div>
        </section>
        <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
</asp:Content>

