<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenericCarousel.ascx.cs" Inherits="GenericControl" %>

<div id="carouselWrapper">

    <div id="imgCarousel" class="carousel slide" data-interval="5000" data-ride="carousel">

        <%--Controls--%>
        <ol class="carousel-indicators">
            <li data-target="#imgCarousel" data-slide-to="0" class="active"></li>
            <li data-target="#imgCarousel" data-slide-to="1"></li>
            <li data-target="#imgCarousel" data-slide-to="2"></li>
        </ol>

        <%--Slides--%>
        <div class="carousel-inner" role="listbox">

            <div class="item active" id="slider1">

                <img src="/cmsimages/93/slide1.jpg" alt="" />
    
                <div class="carousel-caption">
    
                    <h1>Header 2</h1>
    
                    <p>test here</p>

                    <button class="btn btn-danger lrgActionButton">
                        Button
                    </button>

                </div>

            </div>

            <div class="item" id="slider2">

                <img src="/cmsimages/93/slide2.jpg" alt="" />
    
                <div class="carousel-caption">
    
                    <h1>Header 2</h1>
    
                    <p>test here</p>

                    <button class="btn btn-danger lrgActionButton">
                        Button
                    </button>

    
                </div>

            </div>

            <div class="item" id="slider3">

                <img src="/cmsimages/93/slide3.jpg" alt="" />
    
                <div class="carousel-caption">
    
                    <h1>Header 3</h1>
    
                    <p>test here</p>
    
                    <button class="btn btn-danger lrgActionButton">
                        Button
                    </button>

                </div>

            </div>

        </div>


        <%--Prev Next Controls--%>
        <a class="left carousel-control" href="#imgCarousel" role="button" data-slide="prev">
            <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
            <span class="sr-only">Previous</span>
        </a>

        <a class="right carousel-control" href="#imgCarousel" role="button" data-slide="next">
            <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
            <span class="sr-only">Next</span>
        </a>

    </div>

</div>



