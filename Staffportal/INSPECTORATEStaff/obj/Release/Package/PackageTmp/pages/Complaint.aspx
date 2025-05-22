<%@ Page Title="Complaint" Language="C#" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="Complaint.aspx.cs" Inherits="INSPECTORATEStaff.pages.Complaint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1 class="text-primary">Grievance / Complaint</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-home"></i> Home</a></li>
                <li class="active">Complaint</li>
            </ol>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info box-shadow box-rounded box-solid">
                        <div class="box-header with-border bg-info">
                            <h3 class="box-title"><i class="fa fa-diamond"></i> New Complaint</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>

                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label><i class="fa fa-calendar"></i> Grievance Date</label>
                                        <asp:Label ID="lblApplicationDate" CssClass="form-control bg-light text-muted" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label><i class="fa fa-pencil"></i> Description</label>
                                        <asp:TextBox ID="txtDescription" CssClass="form-control" placeholder="Enter complaint details..." TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <asp:Panel ID="details" runat="server" Visible="false">
                                <hr />
                                <p>Resolution Details </p>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label><i class="fa fa-comment"></i> Resolution Notes</label>
                                            <asp:Label ID="lblNotes" CssClass="form-control bg-light text-muted" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label><i class="fa fa-user"></i> Handled By</label>
                                            <asp:Label ID="lblHandledBy" CssClass="form-control bg-light text-muted" runat="server" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label><i class="fa fa-calendar-check-o"></i> Resolution Date</label>
                                            <asp:Label ID="lblDate" CssClass="form-control bg-light text-muted" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label><i class="fa fa-comments-o"></i> Resolution Feedback</label>
                                            <asp:Label ID="lblFeedback" CssClass="form-control bg-light text-muted" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <div class="row mt-4">
                                <div class="col-md-12 d-flex justify-content-between">
                                    <a href="ComplaintListing.aspx" class="btn btn-warning">
                                        <i class="fa fa-arrow-left"></i> Back
                                    </a>
                                    <asp:LinkButton ID="lbtnSubmit" CssClass="btn btn-success pull-right" runat="server" OnClick="lbtnSubmit_Click">
                                        <i class="fa fa-paper-plane"></i> Submit
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div> <!-- box-body -->
                    </div> <!-- box -->
                </div> <!-- col -->
            </div> <!-- row -->
        </section>
    </div>
</asp:Content>
