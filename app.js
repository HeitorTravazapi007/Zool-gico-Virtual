// Função para verificar se o usuário está autenticado
function checkAuthentication() {
    // Verifique se o usuário está autenticado comparando com um valor no localStorage
    const isAuthenticated = localStorage.getItem('authenticated');

    // Se isAuthenticated for verdadeiro, o usuário está autenticado
    return isAuthenticated === 'true';
}

// Verificar a autenticação antes de permitir o acesso à dashboard
window.addEventListener('DOMContentLoaded', () => {
    if (!checkAuthentication()) {
        // Se o usuário não estiver autenticado, redirecione para a página de login
        window.location.href = 'login.html'; 
    }
});

function menu() {
  var x = document.getElementById("myTopnav");
  if (x.className === "topnav") {
    x.className += " responsive";
  } else {
    x.className = "topnav";
  }
}