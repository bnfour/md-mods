; reloads the source image every time it's called
(define (do-the-thing params)
    (let* (
            (in-filename "sprites.png")
            ; still have no idea why this form seems to be required ¯\_(ツ)_/¯
            (in-image (car (gimp-file-load RUN-NONINTERACTIVE in-filename in-filename)))
            ; deconstruct the parameters for usage
            (target-size (car params))
            (filename-resolution (cadr params))

            (out-filename (string-append "../Resources/sprites." filename-resolution ".png"))
        )
        (gimp-image-scale in-image target-size target-size)
        (file-png-export
            #:run-mode RUN-NONINTERACTIVE
            #:image in-image
            #:file out-filename
            #:compression 9
            ; disable optional metadata chunks emitted by default
            #:bkgd FALSE
            #:offs FALSE
            #:phys FALSE
            #:time FALSE
            ; other metadata is disabled by default
        )
        ; prevents leakage of GeglBuffers (EEEEeEeek!)
        (gimp-image-delete in-image)
    )
)

(let* (
        ; first is actual spritesheet size, second is associated display resolution:
        ; only used to form filename, so is defined as a string
        (out-resolutions '((216 "720") (320 "1080") (424 "1440") (640 "2160")))
    )
    (gimp-context-set-interpolation INTERPOLATION-CUBIC)
    (for-each do-the-thing out-resolutions)
    (gimp-quit 0)
)
