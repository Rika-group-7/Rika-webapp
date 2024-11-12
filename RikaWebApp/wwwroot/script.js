const toggleSearch = () => {
    const searchForm = document.querySelector('.search-form')
    const searchInput = document.querySelector('.search-input')
    const searchButton = document.querySelector('.search-button')

    //Kontroll av Elementens Existens
    if (searchForm && searchInput && searchButton) {  
        searchButton.addEventListener('click', () => {
            searchForm.classList.toggle('active-search');
            console.log(searchForm.classList.contains('active-search'));
        });

        searchInput.addEventListener('keydown', (e) => {
            if (e.key === 'Enter') {
                e.preventDefault();
                searchInput.value = '';
                searchForm.classList.remove('active-search');
            }
        });
    }
};

toggleSearch();



window.setCookie = function (name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
};

window.blazorGetCookie = function (name) {
    var match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    if (match) {
        return match[2];
    }
    return null;
};

window.deleteCookie = function (name) {
    document.cookie = name + '=; Max-Age=0; path=/';
};