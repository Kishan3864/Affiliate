// BestPicksHub - Site JavaScript

// Mobile Menu Toggle
function toggleMobileMenu() {
    const navLinks = document.getElementById('navLinks');
    const menuBtn = document.querySelector('.mobile-menu-btn');
    navLinks.classList.toggle('active');
    menuBtn.classList.toggle('active');
}

// Sticky Header
(function () {
    const header = document.querySelector('.site-header');
    if (!header) return;
    let lastScroll = 0;

    window.addEventListener('scroll', function () {
        const currentScroll = window.pageYOffset;
        if (currentScroll > 60) {
            header.classList.add('scrolled');
        } else {
            header.classList.remove('scrolled');
        }
        lastScroll = currentScroll;
    }, { passive: true });
})();

// FAQ Accordion
function toggleFaq(btn) {
    const item = btn.closest('.faq-item');
    const isOpen = item.classList.contains('open');

    // Close all FAQ items
    document.querySelectorAll('.faq-item.open').forEach(function (el) {
        el.classList.remove('open');
    });

    // Open clicked one if it was closed
    if (!isOpen) {
        item.classList.add('open');
    }
}

// Dropdown Menu
(function () {
    document.querySelectorAll('.nav-dropdown').forEach(function (dropdown) {
        dropdown.addEventListener('mouseenter', function () {
            this.querySelector('.dropdown-menu').classList.add('show');
        });
        dropdown.addEventListener('mouseleave', function () {
            this.querySelector('.dropdown-menu').classList.remove('show');
        });

        // Touch support
        dropdown.querySelector('.nav-link').addEventListener('click', function (e) {
            if (window.innerWidth <= 768) {
                e.preventDefault();
                dropdown.querySelector('.dropdown-menu').classList.toggle('show');
            }
        });
    });
})();

// Affiliate Link Click Tracking
(function () {
    document.querySelectorAll('.btn-affiliate, a[rel*="sponsored"]').forEach(function (link) {
        link.addEventListener('click', function () {
            var platform = this.getAttribute('data-platform') || 'unknown';
            var product = this.getAttribute('data-product') || 'unknown';

            // Google Analytics event (if available)
            if (typeof gtag === 'function') {
                gtag('event', 'affiliate_click', {
                    event_category: 'Affiliate',
                    event_label: product,
                    platform: platform
                });
            }
        });
    });
})();

// Smooth Scroll for Table of Contents
(function () {
    document.querySelectorAll('.toc-link, a[href^="#"]').forEach(function (anchor) {
        anchor.addEventListener('click', function (e) {
            var targetId = this.getAttribute('href');
            if (targetId && targetId.startsWith('#')) {
                var target = document.querySelector(targetId);
                if (target) {
                    e.preventDefault();
                    target.scrollIntoView({ behavior: 'smooth', block: 'start' });
                }
            }
        });
    });
})();

// Lazy Image Loading (fallback for browsers without native support)
(function () {
    if ('loading' in HTMLImageElement.prototype) return;

    var images = document.querySelectorAll('img[loading="lazy"]');
    var observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                var img = entry.target;
                img.src = img.dataset.src || img.src;
                observer.unobserve(img);
            }
        });
    });

    images.forEach(function (img) {
        observer.observe(img);
    });
})();

// Close mobile menu on link click
(function () {
    document.querySelectorAll('.nav-links .nav-link').forEach(function (link) {
        link.addEventListener('click', function () {
            var navLinks = document.getElementById('navLinks');
            if (navLinks.classList.contains('active')) {
                navLinks.classList.remove('active');
                document.querySelector('.mobile-menu-btn').classList.remove('active');
            }
        });
    });
})();

// Search input auto-focus on search page
(function () {
    var searchInput = document.querySelector('.hero-search .search-input, .page-header .search-input');
    if (searchInput && window.location.pathname === '/search') {
        searchInput.focus();
    }
})();
