; reloads the source image every time it's called
(define (do-the-thing params)
    (let* (
            (in-filename "sprites.png")

            ; this is here so i won't have to figure it out again, might be too verbose,
            ; but better safe than sorry

            ; load _the_ source image, get its id as the first (and only) element of the list gimp-file-load returned
            (in-image (car (gimp-file-load RUN-NONINTERACTIVE in-filename)))
            ; gimp-image-get-layers returns a list that contains a vector of layer ids:
            ; we only have a single layers, so we get it from the vector by index 0
            (drawable (vector-ref (car (gimp-image-get-layers in-image)) 0))

            ; deconstruct the parameters for usage
            (target-size (car params))
            (filename-resolution (cadr params))

            (out-filename (string-append "../Resources/sprites." filename-resolution ".png"))
        )
        (gimp-image-scale in-image target-size target-size)
        ; clear up image a bit by making the pixels with alpha less that 16 (16/256 = 0.0625) completely transparent;
        ; exporting to PNG does not preserve the pixel's color if alpha is 0, it's replaced by #00000000
        (gimp-drawable-curves-spline drawable HISTOGRAM-ALPHA #(0 0 0.0625 0 0.0625 0.0625 1 1))
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
