// Comportamiento liviano en cliente para menú móvil y cierre de alertas
document.addEventListener('DOMContentLoaded', () => {
    const toggleBtn = document.getElementById('mobile-menu-toggle');
    const nav = document.getElementById('sidebar-nav');

    if (toggleBtn && nav) {
        toggleBtn.addEventListener('click', () => {
            nav.classList.toggle('hidden');
            nav.classList.toggle('flex');
        });
    }

    document.querySelectorAll('[data-dismiss-alert]').forEach((btn) => {
        btn.addEventListener('click', () => {
            const container = btn.closest('[role="status"], [role="alert"]');
            if (container) {
                container.remove();
            }
        });
    });
});
