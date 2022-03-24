const url = 'https://localhost:7168';
const clientId = '060fa59139564c2c956a50ca065af511';

const unauthozireState = 0;
const authozireState = 1;

window.onload = async () => {
	var userState = (await GetUserInfo()).state;
	if(true){
		await SetAuthorizeModule();
	}
}

async function GetUserInfo(){
	var responce = await fetch(url + '/api/users/GetUserInfo');
	return await responce.json();
}

async function SetAuthorizeModule(){
	let body = document.getElementsByTagName('body')[0];
	
	for(let i = 0; i < body.children.length; i++)
		body.children[i].remove();
	
	let form = document.createElement('form');
	form.id = 'auth_form';
	
	let text_input_clientId = document.createElement('input');
	text_input_clientId.type = 'text';
	text_input_clientId.name = 'ClientId';
	text_input_clientId.placeholder = 'Enter client Id';
	text_input_clientId.value = clientId;
	form.append(text_input_clientId);
	
	let submit_input = document.createElement('input');
	submit_input.type = 'submit';
	submit_input.value = 'submit';
	form.append(submit_input);
	
	body.append(form);
	
	auth_form.addEventListener('submit', async() => {
		
		let formData = new FormData(auth_form);
		if(formData.get('ClientId') == '')
			return;
		
		let uri = url + '/api/auth/GetAuthLink';
		let responce = await fetch(uri,{
			method : 'post',
			body: formData
		})
		
		if(responce.status == 200){
			let auth_uri = await responce.json();
			window.open(auth_uri);
		}
	});
}