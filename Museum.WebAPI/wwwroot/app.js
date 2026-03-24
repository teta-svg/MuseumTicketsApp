const API_BASE = "https://localhost:5001/api"; // твой бэкенд
let jwtToken = localStorage.getItem("jwtToken");

// --- Главная страница ---
document.addEventListener("DOMContentLoaded", ()=> {
    if(document.getElementById("exhibitionsContainer")) loadExhibitions();
    setupAuthModals();
    document.getElementById("applyFilters")?.addEventListener("click", ()=> loadExhibitions(getFilters()));
    if(jwtToken) document.getElementById("logoutBtn").style.display="inline-block";
});

function getFilters() {
    return {
        Name: document.getElementById("filterName").value,
        MuseumName: document.getElementById("filterMuseum").value,
        MuseumComplexName: document.getElementById("filterComplex").value,
        StartDate: document.getElementById("filterStart").value,
        EndDate: document.getElementById("filterEnd").value,
        MinPrice: document.getElementById("filterMinPrice").value,
        MaxPrice: document.getElementById("filterMaxPrice").value
    };
}

async function loadExhibitions(filters={}) {
    let url = `${API_BASE}/exhibitions/filter?`;
    Object.entries(filters).forEach(([k,v]) => { if(v) url+=`${k}=${encodeURIComponent(v)}&`; });
    try {
        const res = await fetch(url);
        const data = await res.json();
        const container = document.getElementById("exhibitionsContainer");
        container.innerHTML = "";
        data.forEach(ex => {
            const card = document.createElement("div");
            card.className = "exhibition-card";
            card.innerHTML = `<img src="${ex.photo || 'placeholder.jpg'}" /><h3>${ex.name}</h3><p>${ex.museumName}</p><p>от ${ex.minPrice} ₽</p>`;
            card.addEventListener("click", ()=> { localStorage.setItem("currentExhibitionId", ex.exhibitionID); window.location.href="exhibition.html"; });
            container.appendChild(card);
        });
    } catch(err){ console.error(err); }
}

// --- Детальная выставка ---
document.addEventListener("DOMContentLoaded", async ()=> {
    const detailContainer = document.getElementById("exhibitionDetail");
    if(!detailContainer) return;
    const id = localStorage.getItem("currentExhibitionId");
    if(!id) return;
    try {
        const res = await fetch(`${API_BASE}/exhibitions/${id}`);
        const ex = await res.json();
        detailContainer.innerHTML = `
            <img src="${ex.photo || 'placeholder.jpg'}">
            <h2>${ex.name}</h2>
            <p><b>Музей:</b> ${ex.museumName}</p>
            <p><b>Комплекс:</b> ${ex.museumComplexName || ''}</p>
            <p><b>Дата проведения:</b> ${new Date(ex.startDate).toLocaleDateString()} - ${new Date(ex.endDate).toLocaleDateString()}</p>
            <h3>Билеты:</h3>
            ${ex.tickets.map(t => `<div class="ticket-block"><span>${t.type}</span><span>${t.price} ₽</span><button onclick="buyTicket(${t.ticketId},${t.price})">Купить</button></div>`).join("")}
        `;
    } catch(err){ console.error(err); }
});

// --- Покупка билета ---
async function buyTicket(ticketId, price){
    if(!jwtToken){ alert("Сначала войдите!"); return; }
    try{
        const orderDto = { ExhibitionId: parseInt(localStorage.getItem("currentExhibitionId")), Tickets:[{TicketId: ticketId, Quantity:1}]};
        const orderRes = await fetch(`${API_BASE}/orders`, { method:"POST", headers:{ "Content-Type":"application/json", "Authorization":`Bearer ${jwtToken}` }, body:JSON.stringify(orderDto) });
        const order = await orderRes.json();
        const payRes = await fetch(`${API_BASE}/payments/${order.orderId}`, { method:"POST", headers:{ "Content-Type":"application/json", "Authorization":`Bearer ${jwtToken}` }, body:JSON.stringify(price) });
        const pay = await payRes.json();
        alert(`Оплата: ${pay.status}`);
    } catch(err){ console.error(err); alert("Ошибка покупки") }
}

// --- Авторизация ---
function setupAuthModals(){
    const loginModal = document.getElementById("loginModal");
    const registerModal = document.getElementById("registerModal");

    document.getElementById("loginBtn")?.addEventListener("click", ()=> loginModal.style.display="flex");
    document.getElementById("registerBtn")?.addEventListener("click", ()=> registerModal.style.display="flex");
    document.getElementById("closeLogin")?.addEventListener("click", ()=> loginModal.style.display="none");
    document.getElementById("closeRegister")?.addEventListener("click", ()=> registerModal.style.display="none");

    document.getElementById("loginSubmit")?.addEventListener("click", async ()=>{
        const email=document.getElementById("loginEmail").value;
        const pass=document.getElementById("loginPassword").value;
        try{
            const res=await fetch(`${API_BASE}/auth/login`, { method:"POST", headers:{"Content-Type":"application/json"}, body:JSON.stringify({email,password:pass}) });
            if(res.ok){ const data=await res.json(); localStorage.setItem("jwtToken",data.token); alert("Вход успешен!"); loginModal.style.display="none"; location.reload(); }
            else{ const err=await res.json(); alert(err.message); }
        }catch(e){console.error(e);}
    });

    document.getElementById("registerSubmit")?.addEventListener("click", async ()=>{
        const user={
            email:document.getElementById("regEmail").value,
            password:document.getElementById("regPassword").value,
            firstName:document.getElementById("regFirstName").value,
            lastName:document.getElementById("regLastName").value
        };
        try{
            const res=await fetch(`${API_BASE}/auth/register`, { method:"POST", headers:{"Content-Type":"application/json"}, body:JSON.stringify(user)});
            if(res.ok){ alert("Регистрация прошла успешно!"); registerModal.style.display="none"; }
            else{ const err=await res.json(); alert(err.message); }
        }catch(e){console.error(e);}
    });

    document.getElementById("logoutBtn")?.addEventListener("click", ()=>{
        localStorage.removeItem("jwtToken"); location.reload();
    });
}