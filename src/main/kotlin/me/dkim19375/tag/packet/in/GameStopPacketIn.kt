package me.dkim19375.tag.packet.`in`

import io.ktor.http.cio.websocket.*
import me.dkim19375.tag.main
import me.dkim19375.tag.multiplayer.ClientManager
import me.dkim19375.tag.multiplayer.Profile
import me.dkim19375.tag.multiplayer.ServerManager
import me.dkim19375.tag.packet.Packet

class GameStopPacketIn : Packet {
    override suspend fun execute(
        socket: WebSocketSession,
        text: String?,
        manager: ClientManager
    ): Pair<Profile?, Profile?> {
        main.gameView.stop()
        return Pair(manager.profile, manager.otherProfile)
    }

    override suspend fun execute(
        socket: WebSocketSession,
        text: String?,
        manager: ServerManager
    ): Pair<Profile?, Profile?> {
        main.gameView.stop()
        return Pair(manager.profile, manager.otherProfile)
    }
}