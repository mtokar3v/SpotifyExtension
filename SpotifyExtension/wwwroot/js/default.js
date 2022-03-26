const url = 'https://localhost:7168';
const clientId = '060fa59139564c2c956a50ca065af511';

const unauthozireState = 0;
const authozireState = 1;

window.onload = async () => {
	var userState = (await GetUserInfo()).state;
	if(userState == unauthozireState){
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
	form.className = 'row justify-content-center';
	
	let form_input_wrapper = document.createElement('div');
	form_input_wrapper.className = 'row align-items-center';
	
	let col_clientId_label = document.createElement('div');
	col_clientId_label.className = 'col-3';
	
	let clientId_label = document.createElement('label');
	clientId_label.className = 'col-form-label';
	clientId_label.innerHTML = 'Client Id:';
	
	col_clientId_label.append(clientId_label);
	form_input_wrapper.append(col_clientId_label);
	
	let col_clientId_input = document.createElement('div');
	col_clientId_input.className = 'col-9';
	
	let text_input_clientId = document.createElement('input');
	text_input_clientId.type = 'text';
	text_input_clientId.name = 'ClientId';
	text_input_clientId.placeholder = 'Enter your client Id';
	text_input_clientId.value = clientId;
	text_input_clientId.className = 'form-control';
	text_input_clientId.required = true;
	
	col_clientId_input.append(text_input_clientId);
	form_input_wrapper.append(col_clientId_input);
	form.append(form_input_wrapper);
		
	let form_sumbit_wrapper = document.createElement('div');
	form_sumbit_wrapper.className = 'row align-items-center';
	
	let col_submit = document.createElement('div');
	col_submit.className = 'col-12';
	
	let submit_input = document.createElement('input');
	submit_input.type = 'submit';
	submit_input.value = 'Submit';
	submit_input.className = 'btn btn-primary col-12';
	
	col_submit.append(submit_input);
	form_sumbit_wrapper.append(col_submit);
	form.append(form_sumbit_wrapper);
	
	body.append(form);
	
	auth_form.addEventListener('submit', async(e) => {
		e.preventDefault();
		
		let formData = new FormData(auth_form);
		let clientId = formData.get('ClientId');
		if(clientId == '')
			return;
		
		let responce = await fetch(url + `/api/auth/GetAuthLink?ClientId=${clientId}`);
		if(responce.status == 200){
			window.open(await responce.json());
		}
	});
}