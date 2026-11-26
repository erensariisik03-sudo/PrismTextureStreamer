# Voice & Myinstants update

Base: GMM_Stable_AI_Sirali_Gecis_FIX.zip. Existing model order/timeout, gameplay, wardrobe, chat and player-limit code unchanged.

- Myinstants favorites: normalize titles with line breaks, load/save in app-private files directory (Android Context fallback), retain in-memory changes when atomic save fails and retry every 2 seconds. Separate save status. If the directory is not yet available the UI asks to retry; successful writes persist across app restarts.
- Myinstants stop: cancel pending play/download result, discard ready clip or stop current soundboard clip, restore mic via existing lease path. AI TTS queue and active AI playback are not canceled. An in-flight HTTP download may still finish in the background; its result is ignored.
- Voice: minimum 3 meaningful words per completed ASR utterance or manual word selection; local Turkish/Latin heuristic skips filler/noise/punctuation. Typed chat unaffected. Short utterances are dropped, not joined across different speakers.
- Automatic speaker selection: all discovered remote voice channels (bounded to 24) have independent VAD segments, one ASR worker consumes utterances sequentially. Manual speaker choice remains single-player. Per-channel preview progress; inactive channel state removed. Existing bounded queues may discard older jobs under heavy load.
- Android TTS voice picker: separate touch-scroll page, refresh, choose a device-provided offline Turkish voice or automatic mode. Male preference depends on the engine exposing a recognizable male name. Speed 60–160% via setSpeechRate, default 100%, tone unchanged. Affects subsequent TTS synthesis, not Myinstants MP3 or already synthesized audio.

Verified with host C++ tests: voice_ai_core_test, voice_ai_runtime_test, soundboard_favorites_test, keyboard_ui_test (5 resolutions). Android branch of voice_ai.cpp syntax-checked with host stubs; chat_bot_http.cpp host/JDK syntax check. Android NDK build and live device/voice-room testing not performed.
