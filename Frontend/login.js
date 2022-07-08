const toggleForm = () => {
    const container = document.querySelector('.container');
    container.classList.toggle('active');
  };
  let user=document.getElementById("fname");
  console.log(user);

//LOGIN PAGE JS
function checkUser()
{ 
	debugger;
	let user=document.getElementById("fname").value;
	
	let password=document.getElementById("password").value;

	var request={
		method:'POST',
		redirect:'follow',
		body: JSON.stringify({ 
			"username": user,
			"password": password,
		}),
		// Adding headers to the request
		headers: {
			"Content-type": "application/json; charset=UTF-8"
		}
		
	};
	fetch("http://localhost:59689/api/Login", request)
	.then( res=>{
       return res.json();})
	    .then(data=> showstorage(data))
.catch(error=>console.log(error))

};

function showstorage(data)
{
if(data!=null && data!=undefined && data!="")
{   
	console.log(data);
	sessionStorage.setItem("token",data.token);
	sessionStorage.setItem("userid",data.userid);
	sessionStorage.setItem("username",data.username);
}
loc();
}
function loc()
{
if(sessionStorage.getItem("token")!=null)
{
// alert("Succesful");
window.location.href="dashboard.html";
}
else
{
alert("Login Credentials are wrong");
}

}


//REGISTRATION PAGE JS
function sendData(){
	debugger;
	let user=document.getElementById("userName").value;
	let password=document.getElementById("Password").value;
	var curr=new Date();
	var DateTime=curr.getFullYear()+"-"+curr.getMonth()+"-"+curr.getDay()+" "+ curr.getHours() + ":" 
	+ curr.getMinutes() + ":" + curr.getSeconds();
	console.log(DateTime);
  
	var request={
  
	  method:'POST',
	  redirect:'follow',
	  body: JSON.stringify({
  
		"Username": user,
		"UserPassword":password,
		// "password":CryptoJS.MD5(password).toString(),
		"CreatedAt": DateTime
	  }),
  
	  // Adding headers to the request
	  headers: {
		"Content-type": "application/json; charset=UTF-8"
	  }
	};
	fetch("http://localhost:59689/api/User", request)
	.then(response => response.text())
	.then(result => console.log(result))
	.catch(error => console.log('error', error));}


	// login indicate to dashboard
	function dasboard(){
	window.href="dashboard.html";
	}