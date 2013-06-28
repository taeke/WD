//@author = Taeke van der Veen

"use strict";

// Een pseudo namespace
var wdNS = wdNS || {};

// Koppelt alle code en variabelen aan een pseudo namespace.
//
// @param (context) de pseudo namespace.
(function(context) {

  var iRatio = 1;
  var iOffsetX = 1;
  var iOffsetY = 1;
  var oCTX;
  var iCanvasWidth;
  var iCanvasHeight;
  
  var createDrawingArea = function() {
    var oCanvas = document.getElementById("map");
    if (!oCanvas) {
      alert("Error: missing or incorrect canvas 'id'");
    }
    oCanvas.style.backgroundColor = "#ffffff";

    // get canvas dimensions
    iCanvasWidth = oCanvas.width;
    iCanvasHeight = oCanvas.height;

    oCTX = (oCanvas).getContext('2d');
    oCTX.clearRect(0, 0, iCanvasWidth, iCanvasHeight);
    oCTX.lineWidth = 1;
  }
    
  context.draw = function(sCountry, sColor, number) {
    calc();
    internalDraw(sCountry, sColor);
    drawArmies(context.oMap[sCountry].numberEndPoint, context.oMap[sCountry].numberCenterPoint, number);
  }
  
  context.drawAll = function(){
    createDrawingArea();
    calc();
    for (var sCountry in context.oMap) {
      internalDraw(sCountry, "#dddddd"); 
    }
  }
  
  var drawArmies = function(point1, point2, number) {
      oCTX.moveTo((point1[0] * iRatio) - iOffsetX, (point1[1] * iRatio) - iOffsetY);
      oCTX.lineTo((point2[0] * iRatio) - iOffsetX, (point2[1] * iRatio) - iOffsetY);
      oCTX.stroke();

      oCTX.beginPath();
      oCTX.arc((point2[0] * iRatio) - iOffsetX, (point2[1] * iRatio) - iOffsetY, 48 * iRatio, 0, 2 * Math.PI, false);
      oCTX.fillStyle = "White";
      oCTX.fill();
      oCTX.lineWidth = 2;
      oCTX.strokeStyle = "Black";
      oCTX.stroke();
      
      oCTX.fillStyle = "Black";
      oCTX.font = Math.round(66 * iRatio) + "px Arial";
      var textWidth = oCTX.measureText (number);
      oCTX.fillText(number, (point2[0] * iRatio) - iOffsetX - (textWidth.width / 2), (point2[1] * iRatio) - iOffsetY + (27 * iRatio));
  }
  
  var calcMinMax = function (minOrMaxFunc, value1, value2) {
    if (value1 === undefined) {
      return value2;
    }
    
    return minOrMaxFunc(value1, value2);
  }

  var calc = function() {
    // calculate zoom: find map range
    var iMinX;
    var iMaxX;
    var iMinY;
    var iMaxY;
    for (var sCountry in context.oMap) {
      for (var iCoord = 0; iCoord < context.oMap[sCountry].borderPoints.length; iCoord++) {
        iMinX = calcMinMax( Math.min, iMinX, context.oMap[sCountry].borderPoints[iCoord][0] );
        iMaxX = calcMinMax( Math.max, iMaxX, context.oMap[sCountry].borderPoints[iCoord][0] );
        iMinY = calcMinMax( Math.min, iMinY, context.oMap[sCountry].borderPoints[iCoord][1] );
        iMaxY = calcMinMax( Math.max, iMaxY, context.oMap[sCountry].borderPoints[iCoord][1] );
      }
    }

    // calculate zoom ratio
    var iPadding = 20;
    iRatio = Math.min( ((iCanvasWidth - iPadding) / (iMaxX - iMinX)), ((iCanvasHeight - iPadding) / (iMaxY - iMinY)) );

    // calculate zoom offsets
    var iMidX = (iMinX + ((iMaxX - iMinX) / 2));
    var iMidY = (iMinY + ((iMaxY - iMinY) / 2));
    iOffsetX = ((iMidX * iRatio) - (iCanvasWidth / 2));
    iOffsetY = ((iMidY * iRatio) - (iCanvasHeight / 2));
  }
  
  var internalDraw = function(sCountry, sColor) {
    oCTX.fillStyle = sColor;
    oCTX.lineWidth = 1;
    oCTX.strokeStyle = "Black";
    oCTX.beginPath();
    
    oCTX.moveTo((context.oMap[sCountry].borderPoints[0][0] * iRatio) - iOffsetX, (context.oMap[sCountry].borderPoints[0][1] * iRatio) - iOffsetY);
    for (var iCoord = 1; iCoord < context.oMap[sCountry].borderPoints.length; iCoord++) {
      oCTX.lineTo((context.oMap[sCountry].borderPoints[iCoord][0] * iRatio) - iOffsetX, (context.oMap[sCountry].borderPoints[iCoord][1] * iRatio) - iOffsetY);
    }
    oCTX.closePath();
    oCTX.fill();
    oCTX.stroke();
  }
    
})(wdNS);