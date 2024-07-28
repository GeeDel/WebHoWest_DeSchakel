const setup = () => {
    updateindexview();
}


window.addEventListener("load", setup);

const updateindexview = () => {
    let sectionList = document.getElementById("performanceList");

    let alleFilmSections = sectionList.getElementsByClassName("performance");
    // Elementen definiëren met de attributen
    for (let film of alleFilmSections) {

        let linkDiv = film.getElementsByClassName("onthoud");

        let linkElement = document.createElement("a");
        let likeTekst = document.createTextNode("onthoud");
        linkElement.setAttribute("href", "");  // nog aanpassen
        linkElement.classList.add("likeUnlike");
        linkElement.appendChild(likeTekst);
      //  linkElement.innerText = "onthoud";
        linkElement.addEventListener("click", toggleLikeUnlike);
        film.firstElementChild.appendChild(linkElement);
    }
}


const toggleLikeUnlike = (event) => {
    event.preventDefault();
    let bron = event.target;
    let filmSectie = bron.parentElement.parentElement;
    let id = filmSectie.getAttribute("id");
    let zijbalk = document.getElementById("likeList");
    // zoeken naar de gekozen film in de lijst

    let divs = zijbalk.querySelectorAll("div");
    let teZoekenHref = "#" + id;
    let filmSwatchLink = zijbalk.querySelector(`a[href="${teZoekenHref}"]`);

    if (Boolean(filmSwatchLink)) {
        if (bron.innerHTML === 'onthoud') { bron.innerHTML = 'onthouden'; }
        else { bron.innerHTML = 'onthoud'; }
        let filmSwatch = filmSwatchLink.parentElement;
        filmSwatch.classList.toggle("hidden");
        zijbalkZichtbaarJaNee();
    }
    else {
        bron.innerHTML = 'onthouden';
        // variabelen
        let url = filmSectie.getElementsByClassName("performanceImage");
        let src = url[0].getAttribute("src");
        //
        let nieuweDiv = document.createElement("div");
        let afbeeldingLink = document.createElement("a");
        let kleineAfbeelding = document.createElement("img");
        // attributen en class
        nieuweDiv.classList.add("performance");
        nieuweDiv.setAttribute("id", "perf" + id);
        afbeeldingLink.classList.add("link");
        let hRef = `#${id}`;
        afbeeldingLink.setAttribute("href", hRef);
        // ook in alleFilms
        url[0].setAttribute("href", hRef);
        kleineAfbeelding.classList.add('image');
        kleineAfbeelding.setAttribute("src", src);
        kleineAfbeelding.setAttribute("tag", "titel" + id)
        // rel="noopener" target="_blank"
        kleineAfbeelding.setAttribute("target", "_blank");
        kleineAfbeelding.setAttribute("rel","noopener");
        kleineAfbeelding.setAttribute("width", "100%");
        zijbalk.appendChild(nieuweDiv);
        nieuweDiv.appendChild(afbeeldingLink);
        afbeeldingLink.appendChild(kleineAfbeelding);
        nieuweDiv.addEventListener("click",gaNaarFilmInMovielist);
        zijbalkZichtbaarJaNee();
    }
}

const gaNaarFilmInMovielist = (event) => {
    let bron = event.target;
    let currElement = event.currentTarget;
    let zoekId = currElement.firstChild.getAttribute("href");
    location.href = zoekId;
    zijbalkZichtbaarJaNee();
}

const zijbalkZichtbaarJaNee = () => {
    // zijbalk tonen of niet
    let zijbalk = document.getElementById("likeList");
    let aantalVerborgen = zijbalk.querySelectorAll("div .hidden").length;
    let aantalGelikteFilms = zijbalk.querySelectorAll("div").length;
    if (aantalGelikteFilms - aantalVerborgen === 0) {
        zijbalk.classList.add('hidden');
        // doe eigenlijk niets
    } else {
        zijbalk.classList.remove('hidden');
    }
}
