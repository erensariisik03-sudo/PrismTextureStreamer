#include "voice_ai_bridge.h"
#include <cassert>
#include <iostream>
#include <unistd.h>
int main(){
 using namespace myinstants;
 std::vector<Sound> favorites;
 Sound a={"Vine \"Boom\"", "/media/sounds/vine-boom.mp3"}, b={"Türkçe ses", "/media/sounds/bruh.mp3"};
 assert(ToggleFavorite(favorites,a)&&ToggleFavorite(favorites,b));
 assert(IsFavorite(favorites,a.path));
 std::istringstream in(SerializeFavorites(favorites));auto restored=ParseFavorites(in);
 assert(restored.size()==2&&restored[0].title==a.title&&restored[1].title==b.title);
 std::string path="/tmp/gmm_favorites_test_"+std::to_string(getpid());assert(SaveFavorites(path,favorites));
 std::ifstream file(path);assert(ParseFavorites(file).size()==2);std::remove(path.c_str());
 assert(ToggleFavorite(restored,a)&&!IsFavorite(restored,a.path));
 assert(!ToggleFavorite(restored,{"bad","https://bad.com/a.mp3"}));
 voice_ai_bridge::desired.enabled=false;voice_ai_bridge::soundboard.favorites=restored;
 voice_ai_bridge::PlaySound(b.path);assert(voice_ai_bridge::soundboard.play_pending==b.path);
 assert(ToggleFavorite(restored,{"line\nbreak", "/media/sounds/line.mp3"}));assert(restored.back().title=="line break");
 {using namespace voice_ai_bridge;favorites_loaded=true;favorites_path=path; soundboard.favorites=restored;Favorite(a);assert(IsFavorite(soundboard.favorites,a.path)&&!favorites_dirty);std::ifstream check(path);assert(IsFavorite(ParseFavorites(check),a.path));
 favorites_path=path+"/invalid";Favorite(a);assert(!IsFavorite(soundboard.favorites,a.path)&&favorites_dirty);favorites_path=path;{std::lock_guard<std::mutex> lock(mutex);RetryFavoritesLocked();}assert(!favorites_dirty);std::remove(path.c_str());
 soundboard.loading=true;StopSound();assert(soundboard.stop_pending&&soundboard.play_pending.empty());SetSpeechRate(300);assert(speech_rate==160);SetSpeechRate(20);assert(speech_rate==60);SetSpeechRate(100);ChooseVoice("tr-voice");assert(PreferredVoice()=="tr-voice"&&refresh_voices);}
 std::cout<<"Favorites save/reload/remove and playback request with AI off: PASS\n";
}
