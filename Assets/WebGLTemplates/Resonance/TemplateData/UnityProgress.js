function UnityProgress(gameInstance, progress) {
  if (!gameInstance.Module) {
    return;
  }
  if (!gameInstance.loadingBox) {
    gameInstance.loadingBox = document.createElement("div");
    gameInstance.loadingBox.id = "loadingBox";
    gameInstance.loadingBox.style = "width: 960px; height: 600px; background-color: #000; display:block;";
    gameInstance.loadingBox.bgBar = document.createElement("div");
    gameInstance.loadingBox.bgBar.id = "bgBar";
    gameInstance.loadingBox.bgBar.style = "width: 200px; margin-left:-100px; margin-top:300px; position:absolute; left:50%; height: 5px; display:block; background-color:#EEE; border-radius:5px;";
    gameInstance.loadingBox.progressBar = document.createElement("div");
    gameInstance.loadingBox.progressBar.id = "progressBar";
    gameInstance.loadingBox.progressBar.style = "width: 0px; margin-left:-100px; margin-top:300px; position:absolute; left:50%; height: 5px; display:block; background-color:#0B0; border-radius:5px;";
    gameInstance.loadingBox.loadingInfo = document.createElement("div");
    gameInstance.loadingBox.loadingInfo.id = "loadingInfo";
    gameInstance.loadingBox.loadingInfo.style = "color:#EEE;font-family:sans-serif;letter-spacing:5px;position:absolute;width:100%;text-align:center;font-size:11px;margin-top:320px;";
    gameInstance.loadingBox.loadingInfo.innerHTML = "Loading...";
    gameInstance.loadingBox.spinner = document.createElement("img");
    gameInstance.loadingBox.spinner.id = "spinner";
    gameInstance.loadingBox.spinner.src = "TemplateData/spinner.gif";
    gameInstance.loadingBox.spinner.style = "margin-top:260px; margin-left:-25px; position:absolute; width: 50px; height: 50px; display:none;";

    gameInstance.loadingBox.appendChild(gameInstance.loadingBox.bgBar);
    gameInstance.loadingBox.appendChild(gameInstance.loadingBox.progressBar);
    gameInstance.loadingBox.appendChild(gameInstance.loadingBox.loadingInfo);
    gameInstance.loadingBox.appendChild(gameInstance.loadingBox.spinner);
    document.getElementById("webgl").appendChild(gameInstance.loadingBox);
  }
  if (progress < 1) {
    var length = 200 * Math.min(progress, 1);
    document.getElementById("progressBar").style.width = length+"px";
  } else {
    document.getElementById("progressBar").style.display = "none";
    document.getElementById("bgBar").style.display = "none";
    document.getElementById("loadingInfo").innerHTML = "Preparing...";
    document.getElementById("spinner").style.display = "";
  }
}
