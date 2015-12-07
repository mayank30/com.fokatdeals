<%@ Page Title="" Language="C#" MasterPageFile="~/Parent.Master" AutoEventWireup="true" CodeBehind="RechargeNow.aspx.cs" Inherits="com.fokatdeals.RechargeNow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  
     <div class="item" id="first">
        <div class="row">
            <div class="col-md-12">
                <div class="tabbable" id="tabs-337101">
                    <ul class="nav nav-tabs">
                        <li class="active">
                            <a href="#mobile" data-toggle="tab" id="tbMobile">
                                <img src='<%=ResolveUrl("images/mobile.png")%>' />
                                Mobile</a>
                        </li>
                        <li>
                            <a href="#dth" data-toggle="tab" id="tbDth">
                                <img src='<%=ResolveUrl("images/dth.png")%>' />
                                DTH</a>
                        </li>
                        <li>
                            <a href="#dataCard" data-toggle="tab" id="tbDataCard">
                                <img src='<%=ResolveUrl("images/datacard.png")%>' />
                                Data Card</a>
                        </li>

                    </ul>
                    <br />
                    <div class="tab-content">
                        <div class="tab-pane active" id="mobile">
                            <h1>Recharge your mobile</h1>
                            <div class="cd-form">
                                <div id="optRadio" class="form-group">
                                    <label class="radio-inline">
                                        <input type="radio" value="pre" name="optradio" checked>Prepaid</label>
                                    <label class="radio-inline">
                                        <input type="radio" value="post" name="optradio">Postpaid</label>
                                    <span id="optRadioErr" class="cd-error-message">Error message here!</span>
                                </div>
                                <div class="row">
                                    <div class="col-md-7">
                                        <p class="fieldset">
                                            <label class="image-replace cd-mobile" for="mobileNumber"></label>
                                            <input class="full-width has-padding has-border" id="mobileNumber"
                                                type="text" placeholder="Mobile No." maxlength="11">
                                            <span id="mnErr" class="cd-error-message">Error message here!</span>
                                        </p>
                                    </div>
                                    <div class="col-md-5">
                                        <p id="mobileOperators" class="fieldset">
                                            <label class="image-replace cd-operator" for="reset-email">Select Operators</label>
                                            <select class="full-width has-padding has-border">
                                            </select>
                                        </p>
                                    </div>
                                </div>


                                <p class="fieldset">
                                    <label class="image-replace cd-amount" for="reset-email"></label>
                                    <input class="full-width has-padding has-border" id="rechargeAmt" type="text" placeholder="Amount" maxlength="5">
                                    <span id="amtErr" class="cd-error-message">Error message here!</span>
                                </p>
                                <div id="rechargeType" class="form-group">
                                    <label class="radio-inline">
                                        <input type="radio" value="TOP" name="rechargeType">Topup Recharge</label>
                                    <label class="radio-inline">
                                        <input type="radio" value="SPECIAL" name="rechargeType">Special Recharge</label>
                                    <span id="rtErr" class="cd-error-message">Error message here!</span>
                                </div>
                                <button id="btnMobileRecharge" type="submit" class="btn btn-primary">Proceed to Pay</button><br />
                                <div id="rechargeErr" style="color: red; margin-top: 10px;"></div>
                            </div>
                        </div>
                        <div class="tab-pane" id="dth">
                            <h1>Pay your DTH Bill</h1>
                            <div class="cd-form">
                                <p class="fieldset">
                                    <label class="image-replace cd-operator" for="reset-email">Select Operators</label>
                                    <select id="dthOperator" class="full-width has-padding has-border">
                                    </select>
                                </p>
                                <p class="fieldset">
                                    <label class="image-replace cd-username" for="reset-email"></label>
                                    <input class="full-width has-padding has-border" id="dthUserId" type="text" placeholder="Consumer Id">
                                    <span id="dthUserErr" class="cd-error-message">Error message here!</span>
                                </p>

                                <p class="fieldset">
                                    <label class="image-replace cd-amount" for="reset-email"></label>
                                    <input class="full-width has-padding has-border" id="dthAmt" type="text" placeholder="Amount">
                                    <span id="dthAmtErr" class="cd-error-message">Error message here!</span>
                                </p>

                                <button id="btnDth" type="submit" class="btn btn-primary">Proceed to Pay</button><br />
                                <div id="dthErr" style="color: red; margin-top: 10px;"></div>
                            </div>
                        </div>
                        <div class="tab-pane" id="dataCard">
                            <h1>Recharge your Data Card</h1>
                            <div class="cd-form">
                                <p class="fieldset">
                                    <label class="image-replace cd-operator" for="reset-email">Select Operators</label>
                                    <select id="dataCardOperator" class="full-width has-padding has-border">
                                    </select>
                                </p>
                                <p class="fieldset">
                                    <label class="image-replace cd-username" for="reset-email"></label>
                                    <input class="full-width has-padding has-border" id="dcUserId" type="text" placeholder="Consumer Id" >
                                    <span id="dcIdErr" class="cd-error-message">Error message here!</span>
                                </p>

                                <p class="fieldset">
                                    <label class="image-replace cd-amount" for="reset-email"></label>
                                    <input class="full-width has-padding has-border" id="dcAmt" type="text" placeholder="Amount">
                                    <span id="dcAmtErr" class="cd-error-message">Error message here!</span>
                                </p>

                                <button id="dcBtn" type="submit" class="btn btn-primary">Proceed to Pay Data Card Bill</button><br />
                                <div id="dcErr" style="color: red; margin-top: 10px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
     <%--<div class="item" id="sec">
        <%--<img src="http://topup.deitytech.com/website/1368778566_Slide%201.jpg" />
        <img src="http://www.olxrecharge.com/images/r7.jpg" />
    </div>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="server">
    <script src='<%=ResolveUrl("js/customCalls.js")%>'></script>
</asp:Content>
