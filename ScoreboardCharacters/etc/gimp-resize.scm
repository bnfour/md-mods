; i have no idea what i'm doing >_<

; this reloads the source image every time
(define (do-the-thing params)
    (let* (
            (in-filename "sprites.png")
            (in-image (car (gimp-file-load RUN-NONINTERACTIVE in-filename in-filename)))
            (drawable (car (gimp-image-get-active-layer in-image)))
            ; deconstruct the data tuple(?)
            (target-size (car params))
            (filename-resolution (cadr params))

            (out-filename (string-append "../Resources/sprites." filename-resolution ".png"))
        )
        (gimp-image-scale in-image target-size target-size)
        ; 9 is compression (0-9), all the zeroes are optional(?) metadata disabled
        (file-png-save2 RUN-NONINTERACTIVE in-image drawable out-filename out-filename 0 9 0 0 0 0 0 0 0)
        ; prevents leakage of GeglBuffers (EEEEeEeek!)
        (gimp-image-delete in-image)
    )
)

(let* (
        ; first is actual spritesheet size, second is associated display resolution:
        ; only used to form file name, so is defined as a string
        (out-resolutions '((216 "720") (320 "1080") (424 "1440") (640 "2160")))
    )
    (gimp-context-set-interpolation INTERPOLATION-CUBIC)
    (for-each do-the-thing out-resolutions)
    (gimp-quit 0)
)