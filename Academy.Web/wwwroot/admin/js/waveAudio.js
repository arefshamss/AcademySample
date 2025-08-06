
function initWaveAudio() {
    $(".waveAudio").each(function () {

        const containerElement = this;
        const audioUrl = $(containerElement).data('src');

        const playElement  = $("<span class='play-btn btn btn-info rounded-circle w-40 h-40 d-flex align-items-center justify-content-center ms-3'><i class='fa fa-play'></i></span>");
        $(containerElement).append(playElement);
        const playBtn = $(containerElement).find(".play-btn");
        console.log(playBtn);
        const wavesurfer = WaveSurfer.create({
            container: this,
            url: audioUrl,
            height: 60,
            width: '135px',
            splitChannels: false,
            normalize: false,
            waveColor: '#c6c8ca',
            progressColor: '#4ea9e0',
            cursorWidth: 0,
            barWidth: 2,
            barGap: 1,
            barRadius: 30,
            barHeight: 0.8,
            barAlign: '',
            minPxPerSec: 1,
            fillParent: true,
            mediaControls: false,
            autoplay: false,
            interact: true,
            dragToSeek: false,
            hideScrollbar: false,
            audioRate: 1,
            autoScroll: true,
            autoCenter: true,
            sampleRate: 48000,
        })

        wavesurfer.on('interaction', () => {
            wavesurfer.play();
            playBtn.html('<i class="fa fa-pause"></i>')
        });

        wavesurfer.on('finish', function () {
            playBtn.html('<i class="fa fa-play"></i>');
        });


        playBtn.click(function (e) {
            e.preventDefault();
            if (wavesurfer.isPlaying()) {
                wavesurfer.pause();
                playBtn.html('<i class="fa fa-play"></i>');
            } else {
                wavesurfer.play();
                playBtn.html('<i class="fa fa-pause"></i>')
            }
        });
    })
}

$(() =>{
    initWaveAudio();
})