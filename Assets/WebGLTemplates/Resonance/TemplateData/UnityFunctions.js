gameInstance.Module['onRuntimeInitialized'] = function() {
    console.log("Unity is ready!");
    showUnity(true);
};

function showUnity(show) {
    document.getElementById('loadingBox').style.display = show ? 'none' : 'block';
    document.getElementById('gameContainer').style.display = show ? 'block' : 'none';
}
