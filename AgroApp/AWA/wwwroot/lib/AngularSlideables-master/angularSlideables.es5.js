'use strict';

angular.module('angularSlideables', []).directive('slider', function () {
    return {
        restrict: 'A',
        compile: function compile(element) {
            // wrap tag
            var contents = element.html();
            element.html('<div class="slideable_content" style=" position="relative" margin:0 !important; padding:0 !important" >' + contents + '</div>');

            return function postLink(scope, element, attrs) {
                var i = 0;
                // default properties
                scope.$watch(attrs.slider, function (n, o) {
                    if (n !== o || attrs.enabled) {
                        i++;
                        var target = element[0],
                            content = target.querySelector('.slideable_content');
                        if (n) {
                            content.style.border = '1px solid rgba(0,0,0,0)';
                            var y = content.clientHeight,
                                z = i;
                            content.style.border = 0;
                            target.style.height = y + 'px';
                            setTimeout(function () {
                                if (z === i) {
                                    target.style.height = 'auto';
                                }
                            }, 500);
                        } else {
                            target.style.height = target.clientHeight + 'px';
                            setTimeout(function () {
                                target.style.height = '0px';
                            });
                        }
                    }
                });

                attrs.duration = !attrs.duration ? '0.5s' : attrs.duration;
                attrs.easing = !attrs.easing ? 'ease-in-out' : attrs.easing;
                element.css({
                    'overflow': 'hidden',
                    'height': '0px',
                    'transitionProperty': 'height',
                    'transitionDuration': attrs.duration,
                    'transitionTimingFunction': attrs.easing
                });
            };
        }
    };
});

