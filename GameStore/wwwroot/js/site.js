// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.querySelectorAll('.textarea-wrapper').forEach(wrapper => {
    const textarea = wrapper.querySelector('textarea');
    textarea.addEventListener('input', () => {
        wrapper.dataset.value = textarea.value;
    });
    // Populate on load for Edit view
    wrapper.dataset.value = textarea.value;
});
function syncTextareaWrappers() {
    document.querySelectorAll('.textarea-wrapper').forEach(wrapper => {
        const textarea = wrapper.querySelector('textarea');
        if (!textarea) return;
        textarea.addEventListener('input', () => {
            wrapper.dataset.value = textarea.value;
        });
        wrapper.dataset.value = textarea.value;
    });
}

syncTextareaWrappers();
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

function typeWriterTextArea(element, text, delay = 5) {
    element.value = '';
    let i = 0;
    return new Promise(resolve => {
        const interval = setInterval(() => {
            if (i < text.length) {
                element.value += text[i];
            }
            // Sync the wrapper so CSS grid height updates
            const wrapper = element.closest('.textarea-wrapper');
            if (wrapper) wrapper.dataset.value = element.value;

            i++;
            if (i >= text.length) {
                clearInterval(interval);
                resolve();
            }
        }, delay);
    });
}

const lookupBtn = document.getElementById('lookupBtn');
if (lookupBtn) {
    lookupBtn.addEventListener('click', async function () {
        const name = document.getElementById('gameName').value.trim();
        if (!name) return;

        this.disabled = true;
        this.classList.replace('bi-search', 'bi-cloud-arrow-down');

        try {
            const response = await fetch(`/Igdb/Search?name=${encodeURIComponent(name)}`);
            if (!response.ok) { alert('No game found.'); return; }

            const data = await response.json();

            const animations = [
                typeWriter(document.getElementById('gameName'), data.game.name ?? '')
            ];

            // Always update description (clear it if empty, populate if available)
            const descriptionTextarea = document.querySelector('textarea[name="Game.Description"]');
            if (descriptionTextarea) {
                animations.push(typeWriterTextArea(
                document.querySelector('textarea[name="Game.Description"]'),
                    data.game.description ?? ""
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

            if (data.platformIds?.length > 0) {
                document.querySelectorAll('.platform-check').forEach(cb => cb.checked = false);
                data.platformIds.forEach(id => {
                    const cb = document.getElementById('platform-' + id);
                    if (cb) cb.checked = true;
                });
            }

            await Promise.all(animations);

        } catch (err) {
            alert('Something went wrong during lookup.');
            console.error(err);
        } finally {
            this.disabled = false;
            this.classList.replace('bi-cloud-arrow-down', 'bi-search');
        }
    });
}