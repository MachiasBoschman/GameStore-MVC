// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function typeWriter(element, text, delay = 25) {
    element.value = '';
    let i = 0;
    return new Promise(resolve => {
        const interval = setInterval(() => {
            element.value += text[i];
            i++;
            if (i >= text.length) {
                clearInterval(interval);
                resolve();
            }
        }, delay);
    });
}

function typeWriterTextArea(element, text, delay = 10) {
    element.value = '';
    let i = 0;
    return new Promise(resolve => {
        const interval = setInterval(() => {
            element.value += text[i];
            i++;
            if (i >= text.length) {
                clearInterval(interval);
                resolve();
            }
        }, delay);
    });
}

// Only runs on pages that have a lookup button
const lookupBtn = document.getElementById('lookupBtn');
if (lookupBtn) {
    lookupBtn.addEventListener('click', async function () {
        const name = document.getElementById('gameName').value.trim();
        if (!name) return;

        this.disabled = true;
        this.textContent = 'Looking up...';

        try {
            const response = await fetch(`/Igdb/Search?name=${encodeURIComponent(name)}`);
            if (!response.ok) { alert('No game found.'); return; }

            const data = await response.json();

            const animations = [
                typeWriter(document.getElementById('gameName'), data.game.name ?? '')
            ];

            if (data.game.description) {
                animations.push(typeWriterTextArea(
                    document.querySelector('textarea[name="Game.Description"]'),
                    data.game.description
                ));
            }

            if (data.game.imagePath) {
                document.getElementById('igdbImagePath').value = data.game.imagePath;
                const preview = document.getElementById('imagePreview');
                preview.src = data.game.imagePath;
                preview.style.display = 'block';
            }

            if (data.genreIds?.length > 0)
                $('#GenreIds').val(data.genreIds.map(String)).trigger('change');

            if (data.platformIds?.length > 0)
                $('#PlatformIds').val(data.platformIds.map(String)).trigger('change');

            await Promise.all(animations);

        } catch (err) {
            alert('Something went wrong during lookup.');
            console.error(err);
        } finally {
            this.disabled = false;
            this.textContent = 'Lookup';
        }
    });
}