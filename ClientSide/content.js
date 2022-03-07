var uri = "";

setInterval(checkUri , 2000);

function fillUrl(path){
	return `https://localhost:7168${path}`
}

async function checkUri(){
	
	var pathname = window.location.pathname;
	console.log(pathname);

	var firstArg = pathname.substring(pathname.indexOf('/') + 1, pathname.lastIndexOf('/'));
	var secondArg = pathname.substring(pathname.lastIndexOf('/') + 1);

	console.log(firstArg);
	console.log(secondArg);
	
	var pathToGetPlaylistTracks = fillUrl(`/api/Tracks/GetPlaylistTracks?playlistId=${secondArg}`);
	console.log(pathToGetPlaylistTracks);
	
	var pathToGetSavedTracks = fillUrl(`/api/Tracks/GetUserSavedTracks`);
	console.log(pathToGetSavedTracks);
	
	var responce = await fetch(pathToGetPlaylistTracks);
	if (response.ok) {
		let json = await response.json();
	} else {
		console.log("Ошибка HTTP: " + response.status);
	}
	
	responce = await fetch(pathToGetSavedTracks);
	if (response.ok) {
		let json = await response.json();
	} else {
		console.log("Ошибка HTTP: " + response.status);
	}
}
