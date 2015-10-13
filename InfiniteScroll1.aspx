<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InfiniteScroll1.aspx.cs" Inherits="com.fokatdeals.InfiniteScroll1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>

        body {
            margin:0px;
        }

.container {
  background: #fff;
  width: 100%;
  margin-bottom: 20px;
}

.item {
    width:250px;
    margin : 10px;
  border: 1px solid;
  background: #EEE;
}
        .item img {
            max-width : 250px;
            max-height : 400px;
        }
.item.w2 { height: 120px; }
.item.w3 { height: 180px; }
.item.w4 { height: 240px; }

.item.h2 { height: 100px; }
.item.h3 { height: 160px; }
.item.h4 { height: 220px; }
.item.h5 { height: 280px; }


    </style>
</head>
<body>
   <div id="basic" class="container">

  </div>
    	<script type="text/javascript" src="js/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/masonry/3.3.2/masonry.pkgd.js"></script>
    <script src="http://imagesloaded.desandro.com/imagesloaded.pkgd.js"></script>
    
    	<script type="text/javascript" src="js/productAppend.js"></script>
</body>
</html>
