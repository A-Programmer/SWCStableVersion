<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Courses.ascx.cs" Inherits="Module_CoursesAndAds.Modules.ModuleCoursesAndAds.Blocks.Courses" %>

<div class="five columns widget">
        <div class="five columns" id="upcomingevents">
            <div id="sliderevent">

            <asp:Repeater runat="server" ID="dlCourses">
                <ItemTemplate>
                <div class="slide">
                    <div class="toprow">
                        <div class="datearea"> <strong><%#Eval("CMDay") %></strong> <small><%#Eval("CMMonth") %></small> </div>
                        <div class="four columns contentbox">
                            <h2><asp:HyperLink runat="server" NavigateUrl='<%#Eval("CMID","~/ShowPage/Commers/{0}") %>'><%#Eval("CMTitle") %></asp:HyperLink></h2>
                            <p><%#Eval("CMDescription") %></p>
                        </div>
                    </div>
                </div>
                </ItemTemplate>
            </asp:Repeater>

                
            </div>
            <div class="five columns bottomarea">
                <h2>دوره های آموزشی</h2>
                <div class="controls"> <span id="prev1" class="leftarrow">prev</span> <span id="next1" class="rightarrow">next</span> </div>
            </div>
        </div>
    </div>

