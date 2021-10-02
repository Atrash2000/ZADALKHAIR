﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const counters = document.querySelectorAll("#counter");
const speed = 10;

counters.forEach(
    (counter) => {
        const updateCount = () => {
            const target = +counter.getAttribute('data-target');
            const count = +counter.innerHTML;

            const inc = target / speed;

            /*console.log(inc);*/

            if (count < target) {
                counter.innerHTML = count + inc;
                setTimeout(updateCount, 100);
            }
            else {
                count.innerHTML = target;
            }
        }
        updateCount();
    }
);

var PrevScroll = 0;
const nav = document.querySelector("#nav-container");
const logoImg = document.querySelector("#logo");

function controllNavBar() {
    var currentScroll = pageYOffset;
    if (PrevScroll < currentScroll) {
        nav.classList.add("nav-bg");
        logoImg.classList.remove("logo-img-1");
        logoImg.classList.add("logo-img-2");
    } else {
        nav.classList.remove("nav-bg");
        logoImg.classList.remove("logo-img-2");
        logoImg.classList.add("logo-img-1");
        currentScroll = PrevScroll;
    }
}

/* SideBar */
const sidebarBg = document.querySelector(".sidebar-bg");
const sidebar = document.querySelector("aside");
const btnClose = document.querySelector(".btn-close");

function showSideBar() {
    sidebarBg.classList.add("d-block");
    sidebar.classList.add("display-sidebar");
    btnClose.classList.add("display-close-btn");
}

function closeSidebar() {
    sidebarBg.classList.remove("d-block");
    sidebar.classList.remove("display-sidebar");
    sidebar.style.transition = "right 1s";
    btnClose.classList.remove("display-close-btn");
}