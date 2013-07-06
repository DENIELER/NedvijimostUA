<%@ Page Language="C#" AutoEventWireup="true" CodeFile="3D-room-panorama.aspx.cs" Inherits="_3D_room_panorama" MasterPageFile="~/AdvertismentMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <script src="/js/three.min.js"></script>
    <script src="/js/jquery.masonry.min.js"></script>

    <style type="text/css">
        /*#container {
            background: #ddd;
             -moz-border-radius: 4px;
             border-radius: 4px;
             padding: 0.4em;
             color: rgba(0,0,0, .8);
             text-shadow: 0 1px 0 #fff;
             width: 390px;
             height: 383px;
             position: relative;
             margin-bottom: 50px;
        }*/

            #container:after,
            #container:before {
                z-index: -1;
                position: absolute;
                content: "";
                bottom: 15px;
                left: 10px;
                width: 50%;
                top: 80%;
                max-width:300px;
                background: rgba(0, 0, 0, 0.7);
                -webkit-box-shadow: 0 15px 10px rgba(0,0,0, 0.7);
                -moz-box-shadow: 0 15px 10px rgba(0, 0, 0, 0.7);
                box-shadow: 0 15px 10px rgba(0, 0, 0, 0.7);
                -webkit-transform: rotate(-3deg);
                -moz-transform: rotate(-3deg);
                -o-transform: rotate(-3deg);
                -ms-transform: rotate(-3deg);
                transform: rotate(-3deg);
            }

            #container:after {
                -webkit-transform: rotate(3deg);
                -moz-transform: rotate(3deg);
                -o-transform: rotate(3deg);
                -ms-transform: rotate(3deg);
                transform: rotate(3deg);
                right: 10px;
                left: auto;
            }

        #container {
            background: none repeat scroll 0 0 #FFFFFF;
            box-shadow: 0 1px 3px rgba(34, 25, 25, 0.4);
            margin: 5px;
            padding: 7px;
            text-align: center;
            transition: -moz-box-shadow 1s ease 0s;
            width: 380px;
            position: relative;
            margin-bottom: 40px;
        }
    </style>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <h2>Аренда квартиры</h2>

    <p>
        <strong>Текст объявления:</strong> <br />
        Сдам свою 3х комнатную квартиру в 5ти минутах от м. Киевская 9/10. 
        Хорошее состояние,ТВ, стиральная-автомат,холодильник, микроволновка, Долгосрочно. 
        3700+свет грн/мес т. 050 82704046; 0686085503 Света
    </p>

    <p>
        <strong>Фото:</strong>
    </p>

    <div style="clear: both; width: 100%;">
        <div style="float:left; width: 405px;">
            <div id="container">
            </div>
        </div>
        <div style="float:left; width: 290px;">
            <div data-selector="div.gallery-item" data-target="#modal-gallery" data-toggle="modal-gallery" class="gallery masonry">
                <div data-href="http://www.kharkovforum.com/attachment.php?s=e0c9a879461817c78e86ebdaf2b61e98&amp;attachmentid=7080359&amp;stc=1&amp;amp;d=1371850747" class="gallery-item masonry-brick">
                    <img src="http://www.kharkovforum.com/attachment.php?s=e0c9a879461817c78e86ebdaf2b61e98&amp;attachmentid=7080359&amp;stc=1&amp;thumb=1&amp;d=1371850747">
                </div>
                <div data-href="http://www.kharkovforum.com/attachment.php?s=e0c9a879461817c78e86ebdaf2b61e98&amp;attachmentid=7080360&amp;stc=1&amp;amp;d=1371850747" class="gallery-item masonry-brick">
                    <img src="http://www.kharkovforum.com/attachment.php?s=e0c9a879461817c78e86ebdaf2b61e98&amp;attachmentid=7080360&amp;stc=1&amp;thumb=1&amp;d=1371850747">
                </div>
                <div data-href="http://www.kharkovforum.com/attachment.php?s=e0c9a879461817c78e86ebdaf2b61e98&amp;attachmentid=7080368&amp;stc=1&amp;amp;d=1371850789" class="gallery-item masonry-brick">
                    <img src="http://www.kharkovforum.com/attachment.php?s=e0c9a879461817c78e86ebdaf2b61e98&amp;attachmentid=7080368&amp;stc=1&amp;thumb=1&amp;d=1371850789">
                </div>
                <div data-href="http://www.kharkovforum.com/attachment.php?s=e0c9a879461817c78e86ebdaf2b61e98&amp;attachmentid=7080369&amp;stc=1&amp;amp;d=1371850789" class="gallery-item masonry-brick">
                    <img src="http://www.kharkovforum.com/attachment.php?s=e0c9a879461817c78e86ebdaf2b61e98&amp;attachmentid=7080369&amp;stc=1&amp;thumb=1&amp;d=1371850789">
                </div>
                <div data-href="http://www.kharkovforum.com/attachment.php?s=e0c9a879461817c78e86ebdaf2b61e98&amp;attachmentid=7080370&amp;stc=1&amp;amp;d=1371850789" class="gallery-item masonry-brick">
                    <img src="http://www.kharkovforum.com/attachment.php?s=e0c9a879461817c78e86ebdaf2b61e98&amp;attachmentid=7080370&amp;stc=1&amp;thumb=1&amp;d=1371850789">
                </div>
            </div>
        </div>
    </div>

    <script>
        var camera, scene, renderer;

        var fov = 70,
        texture_placeholder,
        isUserInteracting = false,
        onMouseDownMouseX = 0, onMouseDownMouseY = 0,
        lon = 0, onMouseDownLon = 0,
        lat = 0, onMouseDownLat = 0,
        phi = 0, theta = 0;

        init();
        animate();

        function init() {

            var container, mesh;

            container = document.getElementById('container');
            
            //camera = new THREE.PerspectiveCamera(fov, window.innerWidth / window.innerHeight, 1, 1100);
            camera = new THREE.PerspectiveCamera(fov, 380 / 383, 1, 1100);
            camera.target = new THREE.Vector3(0, 0, 0);

            scene = new THREE.Scene();

            mesh = new THREE.Mesh(new THREE.SphereGeometry(500, 60, 40), new THREE.MeshBasicMaterial({ map: THREE.ImageUtils.loadTexture('img/3d-texture.jpg') }));
            mesh.scale.x = -1;
            scene.add(mesh);

            renderer = new THREE.WebGLRenderer();
            //renderer.setSize(window.innerWidth, window.innerHeight);
            renderer.setSize(380, 383);

            container.appendChild(renderer.domElement);

            document.addEventListener('mousedown', onDocumentMouseDown, false);
            document.addEventListener('mousemove', onDocumentMouseMove, false);
            document.addEventListener('mouseup', onDocumentMouseUp, false);
            //document.addEventListener('mousewheel', onDocumentMouseWheel, false);
            //document.addEventListener('DOMMouseScroll', onDocumentMouseWheel, false);

            //

            //window.addEventListener('resize', onWindowResize, false);

        }

        //function onWindowResize() {
        //    camera.aspect = window.innerWidth / window.innerHeight;
        //    camera.updateProjectionMatrix();
        //    renderer.setSize(window.innerWidth, window.innerHeight);
        //}

        function onDocumentMouseDown(event) {
            event.preventDefault();
            isUserInteracting = true;
            onPointerDownPointerX = event.clientX;
            onPointerDownPointerY = event.clientY;
            onPointerDownLon = lon;
            onPointerDownLat = lat;
        }

        function onDocumentMouseMove(event) {
            if (isUserInteracting) {
                lon = (onPointerDownPointerX - event.clientX) * 0.1 + onPointerDownLon;
                lat = (event.clientY - onPointerDownPointerY) * 0.1 + onPointerDownLat;
            }
        }

        function onDocumentMouseUp(event) {
            isUserInteracting = false;
        }

        function onDocumentMouseWheel(event) {
            // WebKit
            if (event.wheelDeltaY) {
                fov -= event.wheelDeltaY * 0.05;
                // Opera / Explorer 9
            } else if (event.wheelDelta) {
                fov -= event.wheelDelta * 0.05;
                // Firefox
            } else if (event.detail) {
                fov += event.detail * 1.0;
            }
            camera.projectionMatrix.makePerspective(fov, window.innerWidth / window.innerHeight, 1, 1100);
            render();
        }

        function animate() {
            requestAnimationFrame(animate);
            render();
        }

        function render() {
            lat = Math.max(-85, Math.min(85, lat));
            phi = THREE.Math.degToRad(90 - lat);
            theta = THREE.Math.degToRad(lon);

            camera.target.x = 500 * Math.sin(phi) * Math.cos(theta);
            camera.target.y = 500 * Math.cos(phi);
            camera.target.z = 500 * Math.sin(phi) * Math.sin(theta);

            camera.lookAt(camera.target);
            /*
            // distortion
            camera.position.x = - camera.target.x;
            camera.position.y = - camera.target.y;
            camera.position.z = - camera.target.z;
            */
            renderer.render(scene, camera);
        }

        //--- format photos
        var $photosContainer = $('.gallery');
        $photosContainer.imagesLoaded(function () {
            $photosContainer.masonry({
                itemSelector: '.gallery-item',
                //columnWidth: 300,
                //gutterWidth: 20
            });
        });
	</script>
</asp:Content>